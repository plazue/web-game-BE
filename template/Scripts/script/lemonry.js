document.addEventListener("DOMContentLoaded", function () {
    // Xử lý chọn các ô Radio List (Highlight item được chọn)
    function setupRadioGroup(containerId) {
        const container = document.getElementById(containerId);
        if (!container) return;

        const options = container.querySelectorAll('.radio-option');
        options.forEach(option => {
            option.addEventListener('click', function () {
                options.forEach(opt => opt.classList.remove('selected'));
                this.classList.add('selected');
                const radio = this.querySelector('input[type="radio"]');
                if (radio) radio.checked = true;
            });
        });
    }

    setupRadioGroup('shippingOptions');
    setupRadioGroup('paymentOptions');
    setupRadioGroup('billingOptions');

    // Xử lý chuyển đổi Vận chuyển / Nhận tại cửa hàng
    const btnDelivery = document.getElementById('btnDelivery');
    const btnPickup = document.getElementById('btnPickup');

    btnDelivery.addEventListener('click', function () {
        btnDelivery.classList.add('active');
        btnPickup.classList.remove('active');
    });

    btnPickup.addEventListener('click', function () {
        btnPickup.classList.add('active');
        btnDelivery.classList.remove('active');
    });
});