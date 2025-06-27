const canvas = document.getElementById('imageCanvas');
const ctx = canvas.getContext('2d');
const brushSizeValue = document.getElementById('brushSizeValue');
const canvasPlaceholder = document.querySelector('.canvas-placeholder');
const textOverlay = document.querySelector('.text-input-overlay');
const textInput = document.getElementById('textInput');
const fontFamily = document.getElementById('fontFamily');
const confirmTextBtn = document.getElementById('confirmText');
const cancelTextBtn = document.getElementById('cancelText');
const lineStyle = document.getElementById('lineStyle');
const moveBtn = document.getElementById('moveBtn');

// Function to set line style
function setLineStyle(ctx) {
    const style = lineStyle.value;
    const size = parseInt(brushSize.value);

    if (style === 'solid') {
        ctx.setLineDash([]);
    } else if (style === 'dashed') {
        ctx.setLineDash([size * 2, size]);
    } else if (style === 'dotted') {
        ctx.setLineDash([size, size]);
    }
}

// Set canvas size to fit the screen
function resizeCanvas() {
    const mainContainer = document.querySelector('.main-container');
    const toolbarHeight = document.querySelector('.toolbar').offsetHeight;
    const availableHeight = mainContainer.offsetHeight - toolbarHeight;

    canvas.width = mainContainer.offsetWidth;
    canvas.height = availableHeight;
}

// Initial resize
resizeCanvas();
window.addEventListener('resize', () => {
    resizeCanvas();
    if (currentImage) {
        drawImage(currentImage);
    }
});

// Tool elements
const imageInput = document.getElementById('imageInput');
const textBtn = document.getElementById('textBtn');
const drawBtn = document.getElementById('drawBtn');
const rectangleBtn = document.getElementById('rectangleBtn');
const circleBtn = document.getElementById('circleBtn');
const triangleBtn = document.getElementById('triangleBtn');
const lineBtn = document.getElementById('lineBtn');
const colorPicker = document.getElementById('colorPicker');
const brushSize = document.getElementById('brushSize');
const saveBtn = document.getElementById('saveBtn');
const clearBtn = document.getElementById('clearBtn');
const undoBtn = document.getElementById('undoBtn');
const arrowBtn = document.getElementById('arrowBtn');
const cloudBtn = document.getElementById('cloudBtn');
const boldBtn = document.getElementById('boldBtn');
const underlineBtn = document.getElementById('underlineBtn');

// State variables
let isDrawing = false;
let currentTool = 'draw';
let startX;
let startY;
let undoStack = [];
let currentImage = null;
let tempCanvas = null;
let textObjects = [];
let selectedText = null;
let isDraggingText = false;
let dragOffsetX = 0;
let dragOffsetY = 0;
let shapes = [];
let currentShape = null;
let selectedShape = null;
let isDraggingShape = false;

// Initialize
drawBtn.classList.add('active');
updateBrushSizeDisplay();
saveState();

// Tool button event listeners
[
    { btn: drawBtn, tool: 'draw' },
    { btn: moveBtn, tool: 'move' },
    { btn: textBtn, tool: 'text' },
    { btn: rectangleBtn, tool: 'rectangle' },
    { btn: circleBtn, tool: 'circle' },
    { btn: triangleBtn, tool: 'triangle' },
    { btn: lineBtn, tool: 'line' },
    { btn: arrowBtn, tool: 'arrow' },
    { btn: cloudBtn, tool: 'cloud' }
].forEach(({ btn, tool }) => {
    btn.addEventListener('click', () => {
        currentTool = tool;
        document.querySelectorAll('.tool-btn').forEach(button => button.classList.remove('active'));
        btn.classList.add('active');
        canvas.style.cursor = tool === 'text' ? 'text' :
            tool === 'move' ? 'move' : 'crosshair';

        // Show/hide format buttons based on text tool
        if (tool === 'text') {
            boldBtn.classList.add('visible');
            underlineBtn.classList.add('visible');
        } else {
            boldBtn.classList.remove('visible');
            underlineBtn.classList.remove('visible');
            // Deselect text when switching tools
            textObjects.forEach(t => t.isSelected = false);
            selectedText = null;
            updateTextFormatButtons();
        }

        // Clear selection when switching tools
        if (tool !== 'move') {
            selectedShape = null;
        }

        redrawCanvas();
    });
});

// Text input handling
confirmTextBtn.addEventListener('click', () => {
    const text = textInput.value.trim();
    if (text) {
        const font = `${brushSize.value * 2}px ${fontFamily.value}`;
        const textObj = new TextObject(text, startX, startY, font, colorPicker.value);
        textObjects.push(textObj);
        redrawCanvas();
        saveState();
    }
    textOverlay.style.display = 'none';
    textInput.value = '';
});

cancelTextBtn.addEventListener('click', () => {
    textOverlay.style.display = 'none';
    textInput.value = '';
});

// Brush size display update
brushSize.addEventListener('input', (e) => {
    updateBrushSizeDisplay();
    updateSelectedObjectStyle();
});

brushSize.addEventListener('change', () => {
    updateBrushSizeDisplay();
    updateSelectedObjectStyle();
    saveState();
});

function updateBrushSizeDisplay() {
    brushSizeValue.textContent = brushSize.value;
}

// Image loading and drawing
function drawImage(img) {
    // Calculate aspect ratios
    const canvasRatio = canvas.width / canvas.height;
    const imageRatio = img.width / img.height;

    let drawWidth, drawHeight, x, y;

    if (imageRatio > canvasRatio) {
        // Image is wider than canvas (relative to height)
        drawWidth = canvas.width;
        drawHeight = canvas.width / imageRatio;
        x = 0;
        y = (canvas.height - drawHeight) / 2;
    } else {
        // Image is taller than canvas (relative to width)
        drawHeight = canvas.height;
        drawWidth = canvas.height * imageRatio;
        x = (canvas.width - drawWidth) / 2;
        y = 0;
    }

    // Clear canvas and draw image
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    ctx.drawImage(img, x, y, drawWidth, drawHeight);
}

imageInput.addEventListener('change', (e) => {
    const file = e.target.files[0];
    if (file && file.type.startsWith('image/')) {
        const reader = new FileReader();

        reader.onload = (event) => {
            const img = new Image();

            img.onload = () => {
                currentImage = img;
                drawImage(img);
                saveState();
            };

            img.onerror = () => {
                alert('Error loading image. Please try another image.');
            };

            img.src = event.target.result;
        };

        reader.onerror = () => {
            alert('Error reading file. Please try again.');
        };

        reader.readAsDataURL(file);
    } else {
        alert('Please select a valid image file.');
    }
});

// Drawing functionality
canvas.addEventListener('mousedown', handleMouseDown);
canvas.addEventListener('mousemove', handleMouseMove);
canvas.addEventListener('mouseup', handleMouseUp);
canvas.addEventListener('mouseout', handleMouseUp);
canvas.addEventListener('touchstart', handleTouch);
canvas.addEventListener('touchmove', handleTouch);
canvas.addEventListener('touchend', stopDrawing);

function createTempCanvas() {
    if (!tempCanvas) {
        tempCanvas = document.createElement('canvas');
    }
    tempCanvas.width = canvas.width;
    tempCanvas.height = canvas.height;
    const tempCtx = tempCanvas.getContext('2d');
    tempCtx.clearRect(0, 0, tempCanvas.width, tempCanvas.height);
    tempCtx.drawImage(canvas, 0, 0);
    return tempCtx;
}

function handleTouch(e) {
    e.preventDefault();
    const touch = e.touches[0];
    const mouseEvent = new MouseEvent(e.type === 'touchstart' ? 'mousedown' : 'mousemove', {
        clientX: touch.clientX,
        clientY: touch.clientY
    });
    canvas.dispatchEvent(mouseEvent);
}

function handleMouseDown(e) {
    const [mouseX, mouseY] = getMousePos(canvas, e);

    // Always check for text selection first
    const clickedText = textObjects.find(t => t.containsPoint(mouseX, mouseY));

    if (clickedText) {
        isDraggingText = true;
        selectedText = clickedText;
        selectedShape = null; // Deselect any selected shape
        textObjects.forEach(t => t.isSelected = (t === clickedText));
        updateTextFormatButtons();

        // Sync controls with selected text
        colorPicker.value = clickedText.color;
        brushSize.value = clickedText.fontSize / 2; // Text size is double brush size
        updateBrushSizeDisplay();

        dragOffsetX = mouseX - clickedText.x;
        dragOffsetY = mouseY - clickedText.y;
        redrawCanvas();
        return;
    }

    if (currentTool === 'move') {
        // Check for shape selection
        const clickedShape = shapes.find(shape => shape.containsPoint(mouseX, mouseY));
        if (clickedShape) {
            selectedShape = clickedShape;
            selectedText = null; // Deselect any selected text
            textObjects.forEach(t => t.isSelected = false);
            isDraggingShape = true;
            startX = mouseX;
            startY = mouseY;
            canvas.style.cursor = 'grabbing';

            // Sync controls with selected shape
            colorPicker.value = clickedShape.style.strokeStyle;
            brushSize.value = clickedShape.style.lineWidth;
            lineStyle.value = clickedShape.style.lineStyle;
            updateBrushSizeDisplay();
        } else {
            selectedShape = null;
        }
        updateTextFormatButtons();
        redrawCanvas();
        return;
    }

    // If no text was clicked and not using move tool, proceed with normal tool behavior
    if (currentTool === 'text') {
        startX = mouseX;
        startY = mouseY;
        textOverlay.style.display = 'flex';
        textInput.focus();
    } else {
        textObjects.forEach(t => t.isSelected = false);
        selectedText = null;
        isDraggingText = false;
        updateTextFormatButtons();
        redrawCanvas();
        startDrawing(e);
    }
}

function handleMouseMove(e) {
    if (isDraggingShape && selectedShape) {
        const [mouseX, mouseY] = getMousePos(canvas, e);
        const dx = mouseX - startX;
        const dy = mouseY - startY;
        selectedShape.move(dx, dy);
        startX = mouseX;
        startY = mouseY;
        redrawCanvas();
        return;
    }

    // Existing mouse move handling
    if (isDraggingText && selectedText) {
        const [mouseX, mouseY] = getMousePos(canvas, e);
        selectedText.x = mouseX - dragOffsetX;
        selectedText.y = mouseY - dragOffsetY;
        redrawCanvas();
    } else if (isDrawing) {
        draw(e);
    }

    // Update cursor based on hovering over text
    if (currentTool !== 'text') {
        const hoveringText = textObjects.some(t => t.containsPoint(mouseX, mouseY));
        canvas.style.cursor = hoveringText ? 'move' : 'crosshair';
    }
}

function handleMouseUp() {
    if (isDraggingShape) {
        isDraggingShape = false;
        canvas.style.cursor = 'move';
        saveState();
    }

    // Existing mouse up handling
    if (isDraggingText) {
        isDraggingText = false;
        saveState();
    }
    stopDrawing();
}

function startDrawing(e) {
    isDrawing = true;
    [startX, startY] = getMousePos(canvas, e);

    if (currentTool === 'text') {
        textOverlay.style.display = 'flex';
        textInput.focus();
        return;
    }

    ctx.beginPath();
    ctx.moveTo(startX, startY);

    if (currentTool !== 'draw') {
        createTempCanvas();
    }
}

function draw(e) {
    if (!isDrawing) return;
    const [mouseX, mouseY] = getMousePos(canvas, e);

    if (currentTool === 'draw') {
        if (!currentShape) {
            currentShape = new Shape('freehand', {
                points: [{ x: startX, y: startY }]
            }, {
                strokeStyle: colorPicker.value,
                lineWidth: brushSize.value
            });
        }
        currentShape.properties.points.push({ x: mouseX, y: mouseY });
        redrawCanvas();
    } else {
        const style = {
            strokeStyle: colorPicker.value,
            lineWidth: brushSize.value
        };

        switch (currentTool) {
            case 'rectangle':
                currentShape = new Shape('rectangle', {
                    x: startX,
                    y: startY,
                    width: mouseX - startX,
                    height: mouseY - startY
                }, style);
                break;
            case 'circle':
                const radius = Math.sqrt(
                    Math.pow(mouseX - startX, 2) + Math.pow(mouseY - startY, 2)
                );
                currentShape = new Shape('circle', {
                    startX,
                    startY,
                    radius
                }, style);
                break;
            case 'triangle':
                currentShape = new Shape('triangle', {
                    startX,
                    startY,
                    endX: mouseX,
                    endY: mouseY,
                    width: mouseX - startX
                }, style);
                break;
            case 'line':
                currentShape = new Shape('line', {
                    startX,
                    startY,
                    endX: mouseX,
                    endY: mouseY
                }, style);
                break;
            case 'arrow':
                currentShape = new Shape('arrow', {
                    startX,
                    startY,
                    endX: mouseX,
                    endY: mouseY
                }, style);
                break;
            case 'cloud':
                currentShape = new Shape('cloud', {
                    x: Math.min(startX, mouseX),
                    y: Math.min(startY, mouseY),
                    width: Math.abs(mouseX - startX),
                    height: Math.abs(mouseY - startY)
                }, style);
                break;
        }
        redrawCanvas();
    }
}

function stopDrawing() {
    if (isDrawing && currentTool !== 'text') {
        if (currentShape) {
            shapes.push(currentShape);
            currentShape = null;
        }
        saveState();
    }
    isDrawing = false;
}

// Helper function to get correct mouse position
function getMousePos(canvas, e) {
    const rect = canvas.getBoundingClientRect();
    const scaleX = canvas.width / rect.width;
    const scaleY = canvas.height / rect.height;

    return [
        (e.clientX - rect.left) * scaleX,
        (e.clientY - rect.top) * scaleY
    ];
}

// Undo functionality
function saveState() {
    const state = {
        imageData: canvas.toDataURL(),
        textObjects: textObjects.map(obj => ({
            text: obj.text,
            x: obj.x,
            y: obj.y,
            font: obj.font,
            color: obj.color,
            isSelected: obj.isSelected
        })),
        shapes: shapes
    };
    undoStack.push(JSON.stringify(state));
    undoBtn.disabled = false;
}

undoBtn.addEventListener('click', () => {
    if (undoStack.length > 1) {
        undoStack.pop(); // Remove current state
        const previousState = JSON.parse(undoStack[undoStack.length - 1]);

        // Load the previous image state
        const img = new Image();
        img.onload = () => {
            ctx.clearRect(0, 0, canvas.width, canvas.height);
            ctx.drawImage(img, 0, 0);

            // Restore text objects
            textObjects = previousState.textObjects.map(obj =>
                new TextObject(obj.text, obj.x, obj.y, obj.font, obj.color)
            );

            // Restore shapes
            shapes = previousState.shapes.map(shape =>
                new Shape(shape.type, shape.properties, shape.style)
            );

            redrawCanvas();
        };
        img.src = previousState.imageData;
    }
    if (undoStack.length <= 1) {
        undoBtn.disabled = true;
    }
});

// Save functionality
saveBtn.addEventListener('click', () => {
    const link = document.createElement('a');
    link.download = 'edited-image.png';
    link.href = canvas.toDataURL();
    link.click();
});

// Clear functionality
clearBtn.addEventListener('click', () => {

    ctx.clearRect(0, 0, canvas.width, canvas.height);
    textObjects = []; // Clear all text objects
    shapes = []; // Clear all shapes
    currentShape = null;
    undoStack = [];
    saveState();
    if (currentImage) {
        drawImage(currentImage);
        canvasPlaceholder.style.display = 'none';
    } else {
        canvasPlaceholder.style.display = 'flex';
    }
}
);

// Add this class definition after the state variables
class TextObject {
    constructor(text, x, y, font, color) {
        this.text = text;
        this.x = x;
        this.y = y;
        this.font = font;
        this.color = color;
        this.isSelected = false;
        this.isBold = false;
        this.isUnderline = false;
        this.fontSize = parseInt(font.match(/\d+/)[0]); // Extract font size from font string
        this.fontFamily = font.replace(/\d+px\s*/, '').trim(); // Extract font family from font string
        this.measureText();
    }

    measureText() {
        ctx.font = this.getFont();
        const metrics = ctx.measureText(this.text);
        this.width = metrics.width;
        this.height = parseInt(this.font) * 1.2; // Approximate height
    }

    getFont() {
        const size = this.font.match(/\d+/)[0];
        const family = this.font.replace(/^\d+px\s*/, '');
        return `${this.isBold ? 'bold' : ''} ${size}px ${family}`.trim();
    }

    updateFont() {
        this.font = `${this.isBold ? 'bold ' : ''}${this.fontSize}px ${this.fontFamily}`;
    }

    draw() {
        // Draw selection box first if selected
        if (this.isSelected) {
            ctx.strokeStyle = '#0095ff';
            ctx.lineWidth = 1;
            ctx.strokeRect(
                this.x - 4,
                this.y - this.height + 4,
                this.width + 8,
                this.height + 8
            );
        }

        // Draw text
        ctx.font = this.getFont();
        ctx.fillStyle = this.color;
        ctx.fillText(this.text, this.x, this.y);

        // Draw underline if enabled
        if (this.isUnderline) {
            const metrics = ctx.measureText(this.text);
            ctx.beginPath();
            ctx.strokeStyle = this.color;
            ctx.lineWidth = Math.max(1, parseInt(this.font) / 20);
            ctx.moveTo(this.x, this.y + 3);
            ctx.lineTo(this.x + metrics.width, this.y + 3);
            ctx.stroke();
        }
    }

    containsPoint(x, y) {
        const padding = 8; // Increased hit area for easier selection
        return x >= this.x - padding &&
            x <= this.x + this.width + padding &&
            y >= this.y - this.height - padding &&
            y <= this.y + padding;
    }
}

// Add this class definition after TextObject class
class Shape {
    constructor(type, properties, style) {
        this.type = type;
        this.properties = properties;
        this.style = style;
        this.style.lineStyle = lineStyle.value;
    }

    draw(ctx) {
        // Store the original line dash setting
        const originalLineDash = ctx.getLineDash();

        // Draw the shape
        ctx.strokeStyle = this.style.strokeStyle;
        ctx.lineWidth = this.style.lineWidth;

        // Apply the stored line style
        if (this.style.lineStyle === 'solid') {
            ctx.setLineDash([]);
        } else if (this.style.lineStyle === 'dashed') {
            ctx.setLineDash([this.style.lineWidth * 2, this.style.lineWidth]);
        } else if (this.style.lineStyle === 'dotted') {
            ctx.setLineDash([this.style.lineWidth, this.style.lineWidth]);
        }

        // Draw the shape based on its type
        switch (this.type) {
            case 'freehand':
                if (this.properties.points.length > 0) {
                    ctx.beginPath();
                    ctx.moveTo(this.properties.points[0].x, this.properties.points[0].y);
                    this.properties.points.forEach(point => {
                        ctx.lineTo(point.x, point.y);
                    });
                    ctx.stroke();
                }
                break;
            case 'rectangle':
                ctx.beginPath();
                ctx.rect(this.properties.x, this.properties.y, this.properties.width, this.properties.height);
                ctx.stroke();
                break;
            case 'circle':
                ctx.beginPath();
                ctx.arc(this.properties.startX, this.properties.startY, this.properties.radius, 0, Math.PI * 2);
                ctx.stroke();
                break;
            case 'triangle':
                ctx.beginPath();
                ctx.moveTo(this.properties.startX, this.properties.startY);
                ctx.lineTo(this.properties.startX + this.properties.width / 2, this.properties.endY);
                ctx.lineTo(this.properties.startX + this.properties.width, this.properties.startY);
                ctx.closePath();
                ctx.stroke();
                break;
            case 'line':
                ctx.beginPath();
                ctx.moveTo(this.properties.startX, this.properties.startY);
                ctx.lineTo(this.properties.endX, this.properties.endY);
                ctx.stroke();
                break;
            case 'arrow':
                const angle = Math.atan2(this.properties.endY - this.properties.startY, this.properties.endX - this.properties.startX);
                ctx.beginPath();
                ctx.moveTo(this.properties.startX, this.properties.startY);
                ctx.lineTo(this.properties.endX, this.properties.endY);
                ctx.stroke();
                drawArrowHead(ctx, this.properties.endX, this.properties.endY, angle, 15);
                break;
            case 'cloud':
                drawCloud(ctx, this.properties.x, this.properties.y, this.properties.width, this.properties.height);
                break;
        }

        // Draw selection indicator if shape is selected
        if (this === selectedShape) {
            ctx.strokeStyle = '#0095ff';
            ctx.lineWidth = 1;
            ctx.setLineDash([5, 3]);

            // Draw selection box based on shape type
            switch (this.type) {
                case 'rectangle':
                case 'cloud':
                    ctx.strokeRect(
                        this.properties.x - 5,
                        this.properties.y - 5,
                        this.properties.width + 10,
                        this.properties.height + 10
                    );
                    break;

                case 'circle':
                    ctx.beginPath();
                    ctx.arc(
                        this.properties.startX,
                        this.properties.startY,
                        this.properties.radius + 5,
                        0,
                        Math.PI * 2
                    );
                    ctx.stroke();
                    break;

                case 'triangle':
                    const padding = 5;
                    ctx.beginPath();
                    ctx.moveTo(this.properties.startX - padding, this.properties.startY - padding);
                    ctx.lineTo(
                        this.properties.startX + this.properties.width / 2,
                        this.properties.endY + padding
                    );
                    ctx.lineTo(
                        this.properties.startX + this.properties.width + padding,
                        this.properties.startY - padding
                    );
                    ctx.closePath();
                    ctx.stroke();
                    break;

                case 'line':
                case 'arrow':
                    const angle = Math.atan2(
                        this.properties.endY - this.properties.startY,
                        this.properties.endX - this.properties.startX
                    );
                    const dx = Math.sin(angle) * 5;
                    const dy = -Math.cos(angle) * 5;

                    ctx.beginPath();
                    ctx.moveTo(this.properties.startX + dx, this.properties.startY + dy);
                    ctx.lineTo(this.properties.endX + dx, this.properties.endY + dy);
                    ctx.moveTo(this.properties.startX - dx, this.properties.startY - dy);
                    ctx.lineTo(this.properties.endX - dx, this.properties.endY - dy);
                    ctx.stroke();
                    break;

                case 'freehand':
                    // Create a path that encompasses all points
                    if (this.properties.points.length > 0) {
                        const padding = 5;
                        let minX = this.properties.points[0].x;
                        let minY = this.properties.points[0].y;
                        let maxX = minX;
                        let maxY = minY;

                        this.properties.points.forEach(point => {
                            minX = Math.min(minX, point.x);
                            minY = Math.min(minY, point.y);
                            maxX = Math.max(maxX, point.x);
                            maxY = Math.max(maxY, point.y);
                        });

                        ctx.strokeRect(
                            minX - padding,
                            minY - padding,
                            maxX - minX + padding * 2,
                            maxY - minY + padding * 2
                        );
                    }
                    break;
            }
        }

        // Restore the original line dash setting
        ctx.setLineDash(originalLineDash);
    }

    containsPoint(x, y) {
        const tolerance = 5; // Tolerance for hit detection

        switch (this.type) {
            case 'rectangle':
                return x >= this.properties.x - tolerance &&
                    x <= this.properties.x + this.properties.width + tolerance &&
                    y >= this.properties.y - tolerance &&
                    y <= this.properties.y + this.properties.height + tolerance;

            case 'circle':
                const dx = x - this.properties.startX;
                const dy = y - this.properties.startY;
                const distance = Math.sqrt(dx * dx + dy * dy);
                return Math.abs(distance - this.properties.radius) <= tolerance;

            case 'triangle':
                // Check if point is near any of the triangle's edges
                const trianglePoints = [
                    { x: this.properties.startX, y: this.properties.startY },
                    { x: this.properties.startX + this.properties.width / 2, y: this.properties.endY },
                    { x: this.properties.startX + this.properties.width, y: this.properties.startY }
                ];

                for (let i = 0; i < trianglePoints.length; i++) {
                    const j = (i + 1) % trianglePoints.length;
                    const dist = pointToLineDistance(x, y, trianglePoints[i].x, trianglePoints[i].y, trianglePoints[j].x, trianglePoints[j].y);
                    if (dist <= tolerance) return true;
                }
                return false;

            case 'line':
            case 'arrow':
                return pointToLineDistance(
                    x, y,
                    this.properties.startX, this.properties.startY,
                    this.properties.endX, this.properties.endY
                ) <= tolerance;

            case 'cloud':
                return x >= this.properties.x - tolerance &&
                    x <= this.properties.x + this.properties.width + tolerance &&
                    y >= this.properties.y - tolerance &&
                    y <= this.properties.y + this.properties.height + tolerance;

            case 'freehand':
                const pathPoints = this.properties.points;
                for (let i = 1; i < pathPoints.length; i++) {
                    const dist = pointToLineDistance(
                        x, y,
                        pathPoints[i - 1].x, pathPoints[i - 1].y,
                        pathPoints[i].x, pathPoints[i].y
                    );
                    if (dist <= tolerance) return true;
                }
                return false;
        }
        return false;
    }

    move(dx, dy) {
        switch (this.type) {
            case 'rectangle':
            case 'cloud':
                this.properties.x += dx;
                this.properties.y += dy;
                break;

            case 'circle':
                this.properties.startX += dx;
                this.properties.startY += dy;
                break;

            case 'triangle':
                this.properties.startX += dx;
                this.properties.endX += dx;
                this.properties.startY += dy;
                this.properties.endY += dy;
                break;

            case 'line':
            case 'arrow':
                this.properties.startX += dx;
                this.properties.startY += dy;
                this.properties.endX += dx;
                this.properties.endY += dy;
                break;

            case 'freehand':
                this.properties.points = this.properties.points.map(point => ({
                    x: point.x + dx,
                    y: point.y + dy
                }));
                break;
        }
    }
}

// Add these helper functions for arrow and cloud drawing
function drawArrowHead(ctx, x, y, radians, size) {
    const oldLineDash = ctx.getLineDash(); // Store current line dash
    ctx.setLineDash([]); // Arrow head should always be solid

    ctx.beginPath();
    ctx.moveTo(x, y);
    ctx.lineTo(
        x - size * Math.cos(radians - Math.PI / 6),
        y - size * Math.sin(radians - Math.PI / 6)
    );
    ctx.moveTo(x, y);
    ctx.lineTo(
        x - size * Math.cos(radians + Math.PI / 6),
        y - size * Math.sin(radians + Math.PI / 6)
    );
    ctx.stroke();

    ctx.setLineDash(oldLineDash); // Restore line dash
}

function drawCloud(ctx, x, y, width, height) {
    const oldLineDash = ctx.getLineDash(); // Store current line dash

    ctx.beginPath();

    // Start from top-right
    ctx.moveTo(x + width * 0.75, y + height * 0.25);

    // Top curve
    ctx.bezierCurveTo(
        x + width * 0.65, y,
        x + width * 0.35, y,
        x + width * 0.25, y + height * 0.25
    );

    // Left side curve
    ctx.bezierCurveTo(
        x + width * 0.05, y + height * 0.25,
        x, y + height * 0.55,
        x + width * 0.15, y + height * 0.7
    );

    // Bottom curve
    ctx.bezierCurveTo(
        x + width * 0.15, y + height * 0.85,
        x + width * 0.35, y + height,
        x + width * 0.5, y + height * 0.9
    );

    // Right-bottom curve
    ctx.bezierCurveTo(
        x + width * 0.65, y + height,
        x + width * 0.85, y + height * 0.85,
        x + width * 0.85, y + height * 0.7
    );

    // Right side curve
    ctx.bezierCurveTo(
        x + width, y + height * 0.55,
        x + width * 0.95, y + height * 0.25,
        x + width * 0.75, y + height * 0.25
    );

    ctx.closePath();
    ctx.stroke();
}

// Update the redrawCanvas function
function redrawCanvas() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);

    // Draw the background image if it exists
    if (currentImage) {
        drawImage(currentImage);
    }

    // Draw all saved shapes
    shapes.forEach(shape => shape.draw(ctx));

    // Draw current shape being drawn
    if (currentShape) {
        currentShape.draw(ctx);
    }

    // Draw all text objects
    textObjects.forEach(textObj => textObj.draw());
}

// Text formatting button event listeners
boldBtn.addEventListener('click', (e) => {
    e.stopPropagation();
    if (selectedText) {
        selectedText.isBold = !selectedText.isBold;
        selectedText.updateFont();
        redrawCanvas();
        saveState();
        boldBtn.classList.toggle('active', selectedText.isBold);
    }
});

underlineBtn.addEventListener('click', (e) => {
    e.stopPropagation();
    if (selectedText) {
        selectedText.isUnderline = !selectedText.isUnderline;
        redrawCanvas();
        saveState();
        underlineBtn.classList.toggle('active', selectedText.isUnderline);
    }
});

function updateTextFormatButtons() {
    const isTextMode = currentTool === 'text';

    // Show buttons if we're in text mode or have selected text
    if (isTextMode || selectedText) {
        boldBtn.style.display = 'flex';
        underlineBtn.style.display = 'flex';

        // Update active states
        if (selectedText) {
            boldBtn.classList.toggle('active', selectedText.isBold);
            underlineBtn.classList.toggle('active', selectedText.isUnderline);
        } else {
            boldBtn.classList.remove('active');
            underlineBtn.classList.remove('active');
        }
    } else {
        boldBtn.style.display = 'none';
        underlineBtn.style.display = 'none';
    }
}

// Helper function to calculate distance from point to line segment
function pointToLineDistance(px, py, x1, y1, x2, y2) {
    const A = px - x1;
    const B = py - y1;
    const C = x2 - x1;
    const D = y2 - y1;

    const dot = A * C + B * D;
    const len_sq = C * C + D * D;
    let param = -1;

    if (len_sq !== 0) {
        param = dot / len_sq;
    }

    let xx, yy;

    if (param < 0) {
        xx = x1;
        yy = y1;
    } else if (param > 1) {
        xx = x2;
        yy = y2;
    } else {
        xx = x1 + param * C;
        yy = y1 + param * D;
    }

    const dx = px - xx;
    const dy = py - yy;

    return Math.sqrt(dx * dx + dy * dy);
}

// Add event listeners for style changes
colorPicker.addEventListener('input', updateSelectedObjectStyle);
colorPicker.addEventListener('change', () => {
    updateSelectedObjectStyle();
    saveState();
});

lineStyle.addEventListener('input', updateSelectedObjectStyle);
lineStyle.addEventListener('change', () => {
    updateSelectedObjectStyle();
    saveState();
});

function updateSelectedObjectStyle() {
    if (selectedShape) {
        selectedShape.style.strokeStyle = colorPicker.value;
        selectedShape.style.lineWidth = parseInt(brushSize.value);
        selectedShape.style.lineStyle = lineStyle.value;
        redrawCanvas();
    } else if (selectedText) {
        selectedText.color = colorPicker.value;
        selectedText.fontSize = parseInt(brushSize.value) * 2; // Double brush size for text
        selectedText.updateFont();
        redrawCanvas();
    }
} 