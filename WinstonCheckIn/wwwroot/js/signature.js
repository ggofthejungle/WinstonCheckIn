// Simple canvas drawing functions that work reliably on Chrome/Windows 10
let isDrawing = false;
let lastX = 0;
let lastY = 0;

window.startDrawing = function(canvasId, x, y) {
    const canvas = document.getElementById(canvasId);
    if (canvas) {
        const rect = canvas.getBoundingClientRect();
        const ctx = canvas.getContext('2d');
        
        isDrawing = true;
        lastX = x - rect.left;
        lastY = y - rect.top;
        
        ctx.beginPath();
        ctx.moveTo(lastX, lastY);
        ctx.strokeStyle = '#000000';
        ctx.lineWidth = 2;
        ctx.lineCap = 'round';
        ctx.lineJoin = 'round';
    }
};

window.draw = function(canvasId, x, y) {
    if (!isDrawing) return;
    
    const canvas = document.getElementById(canvasId);
    if (canvas) {
        const rect = canvas.getBoundingClientRect();
        const ctx = canvas.getContext('2d');
        
        const currentX = x - rect.left;
        const currentY = y - rect.top;
        
        ctx.lineTo(currentX, currentY);
        ctx.stroke();
        
        lastX = currentX;
        lastY = currentY;
    }
};

window.stopDrawing = function(canvasId) {
    isDrawing = false;
};

window.clearCanvas = function(canvasId) {
    const canvas = document.getElementById(canvasId);
    if (canvas) {
        const ctx = canvas.getContext('2d');
        ctx.clearRect(0, 0, canvas.width, canvas.height);
    }
};

window.getCanvasData = function(canvasId) {
    const canvas = document.getElementById(canvasId);
    if (canvas) {
        return canvas.toDataURL('image/png');
    }
    return null;
};

window.isCanvasEmpty = function(canvasId) {
    const canvas = document.getElementById(canvasId);
    if (canvas) {
        const ctx = canvas.getContext('2d');
        const imageData = ctx.getImageData(0, 0, canvas.width, canvas.height);
        const data = imageData.data;
        
        // Check if canvas has any non-transparent pixels
        for (let i = 0; i < data.length; i += 4) {
            if (data[i + 3] !== 0) { // Alpha channel
                return false;
            }
        }
        return true;
    }
    return true;
};

// Print function for check-in records
window.printContent = function(htmlContent, title) {
    const printWindow = window.open('', '_blank', 'width=800,height=600');
    
    if (printWindow) {
        printWindow.document.write(htmlContent);
        printWindow.document.close();
        
        // Wait for images to load before printing
        printWindow.onload = function() {
            setTimeout(() => {
                printWindow.print();
                printWindow.close();
            }, 500);
        };
    } else {
        // Fallback if popup is blocked
        const printDiv = document.createElement('div');
        printDiv.innerHTML = htmlContent;
        printDiv.style.position = 'absolute';
        printDiv.style.left = '-9999px';
        document.body.appendChild(printDiv);
        
        window.print();
        document.body.removeChild(printDiv);
    }
};

// Legacy function for compatibility
export function initializeSignaturePad(canvasId) {
    return new Promise((resolve) => {
        const canvas = document.getElementById(canvasId);
        if (canvas) {
            canvas.width = 400;
            canvas.height = 150;
            console.log('Simple signature pad initialized');
            
            resolve({
                clear: () => window.clearCanvas(canvasId),
                isEmpty: () => window.isCanvasEmpty(canvasId),
                getSignatureData: () => window.getCanvasData(canvasId),
                hasSignature: () => !window.isCanvasEmpty(canvasId)
            });
        }
    });
}
