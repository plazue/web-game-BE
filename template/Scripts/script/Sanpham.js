function openImage(element) {
    const clickedImg = element.querySelector('img');
    const mainImage = document.querySelector('.main-image');
    if (clickedImg && mainImage) {
        mainImage.src = clickedImg.src;
    }
    document.querySelectorAll('.thumb .box').forEach(box => box.classList.remove('active'));
    element.classList.add('active');
}

function increaseQty() {
    const input = document.getElementById('quantity-input');
    if (input) input.value = (parseInt(input.value) || 1) + 1;
}

function decreaseQty() {
    const input = document.getElementById('quantity-input');
    if (input) {
        let val = parseInt(input.value) || 1;
        if (val > 1) input.value = val - 1;
    }
}

document.addEventListener("DOMContentLoaded", function () {
    const addToCartBtn = document.querySelector(".btn-buy-now");
    if (addToCartBtn) {
        addToCartBtn.addEventListener("click", function () {
            const title = document.querySelector(".product-title")?.innerText.trim() || "Sản phẩm";
            const priceText = document.querySelector(".product-price")?.innerText || "0";
            const image = document.querySelector(".main-image")?.getAttribute("src") || "";
            const quantity = parseInt(document.getElementById("quantity-input")?.value) || 1;
            const price = parseInt(priceText.replace(/\D/g, "")) || 0;

            const product = { id: title, title, price, priceFormatted: priceText, image, quantity };

            let cart = JSON.parse(localStorage.getItem("cart")) || [];
            const existingIndex = cart.findIndex(item => item.id === product.id);

            if (existingIndex > -1) {
                cart[existingIndex].quantity += quantity;
            } else {
                cart.push(product);
            }

            localStorage.setItem("cart", JSON.stringify(cart));
            window.location.href = 'GioHang';
        });
    }
});