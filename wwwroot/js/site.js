// FirmSheet AAU ADE Application JavaScript

document.addEventListener('DOMContentLoaded', function() {
    const barcodeInput = document.getElementById('barcodeInput');
    const processButton = document.getElementById('processBarcode');
    const clearButton = document.getElementById('clearInput');

    if (barcodeInput) {
        // Focus on the barcode input when page loads
        barcodeInput.focus();

        // Handle Enter key press
        barcodeInput.addEventListener('keypress', function(e) {
            if (e.key === 'Enter') {
                processBarcode();
            }
        });

        // Auto-uppercase input
        barcodeInput.addEventListener('input', function(e) {
            e.target.value = e.target.value.toUpperCase();
        });
    }

    if (processButton) {
        processButton.addEventListener('click', processBarcode);
    }

    if (clearButton) {
        clearButton.addEventListener('click', function() {
            if (barcodeInput) {
                barcodeInput.value = '';
                barcodeInput.focus();
            }
        });
    }

    function processBarcode() {
        const barcode = barcodeInput?.value?.trim();
        
        if (!barcode) {
            alert('Please enter a barcode to process.');
            return;
        }

        // Basic barcode validation (adjust pattern as needed)
        const barcodePattern = /^[A-Z0-9]{10,30}$/;
        if (!barcodePattern.test(barcode)) {
            alert('Invalid barcode format. Please check the barcode and try again.');
            return;
        }

        // TODO: Implement actual barcode processing
        console.log('Processing barcode:', barcode);
        alert(`Processing barcode: ${barcode}\n\nThis will generate AAU and ADE scans for associated pieces.\n\n(Implementation pending)`);
        
        // Clear the input after processing
        if (barcodeInput) {
            barcodeInput.value = '';
            barcodeInput.focus();
        }
    }
});