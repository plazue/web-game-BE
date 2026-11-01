// 1. Hàm đổi ảnh và cập nhật viền active cho thumbnail
function openImage(element) {
    const clickedImg = element.querySelector('img');
    const mainImage = document.querySelector('.main-image');
    if (clickedImg && mainImage) {
        mainImage.src = clickedImg.src;
    }
}
// 2. Hàm tăng số lượng
function increaseQty() {
    const input = document.getElementById('quantity-input');
    let value = parseInt(input.value) || 1;
    input.value = value + 1;
}

// 3. Hàm giảm số lượng
function decreaseQty() {
    const input = document.getElementById('quantity-input');
    let value = parseInt(input.value) || 1;
    if (value > 1) {
        input.value = value - 1;
    }
}

// 4. Xử lý thêm vào giỏ hàng và chuyển trang
document.addEventListener("DOMContentLoaded", function () {
    const addToCartBtn = document.querySelector(".btn-buy-now");

    if (addToCartBtn) {
        addToCartBtn.addEventListener("click", function () {
            const titleEl = document.querySelector(".product-title");
            const priceEl = document.querySelector(".product-price");
            const imageEl = document.querySelector(".main-image");
            const qtyInput = document.getElementById("quantity-input");

            const title = titleEl ? titleEl.innerText : "Sản phẩm";
            const priceText = priceEl ? priceEl.innerText : "0";
            const image = imageEl ? imageEl.getAttribute("src") : "";
            const quantity = qtyInput ? (parseInt(qtyInput.value) || 1) : 1;

            const productId = title.trim();
            const price = parseInt(priceText.replace(/\D/g, "")) || 0;

            const product = {
                id: productId,
                title: title,
                price: price,
                priceFormatted: priceText,
                image: image,
                quantity: quantity
            };

            let cart = JSON.parse(localStorage.getItem("cart")) || [];
            const existingIndex = cart.findIndex(item => item.id === product.id);

            if (existingIndex > -1) {
                cart[existingIndex].quantity += quantity;
            } else {
                cart.push(product);
            }

            localStorage.setItem("cart", JSON.stringify(cart));

            // Chuyển hướng sang trang giỏ hàng (kiểm tra lại route của bạn)
            window.location.href = 'GioHang';
        });
    }
});