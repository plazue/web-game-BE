document.addEventListener("DOMContentLoaded", function () {
    let cart = JSON.parse(localStorage.getItem("cart")) || [];

    const tbody = document.querySelector(".cart-table tbody");
    const subtotalRowSpan = document.querySelector(".subtotal-row span:last-child");
    const totalPriceSpan = document.querySelector(".total-price");
    const checkoutBtn = document.getElementById("checkout-btn") || document.querySelector(".checkout-btn");
    const orderNote = document.getElementById("order-note") || document.querySelector(".note-box textarea");

    // Trường hợp giỏ hàng trống
    if (cart.length === 0) {
        if (tbody) {
            tbody.innerHTML = `<tr><td colspan="3" style="text-align: center; padding: 40px; color: #666;">Giỏ hàng của bạn đang trống</td></tr>`;
        }
        if (subtotalRowSpan) subtotalRowSpan.innerText = "0₫";
        if (totalPriceSpan) totalPriceSpan.innerText = "0 VND";

        if (checkoutBtn) {
            checkoutBtn.style.backgroundColor = "#ccc";
            checkoutBtn.style.cursor = "not-allowed";
            checkoutBtn.style.opacity = "0.7";
            checkoutBtn.disabled = true;
        }
        return;
    }

    // Trường hợp giỏ hàng có sản phẩm
    let html = "";
    let subtotal = 0;

    cart.forEach((item, index) => {
        let priceNum = Number(item.price) || 0;
        let qtyNum = Number(item.quantity) || 1;
        let itemTotal = priceNum * qtyNum;
        subtotal += itemTotal;

        let formattedItemTotal = itemTotal.toLocaleString('vi-VN') + "₫";

        html += `
            <tr>
                <td class="col-product">
                    <div class="product-info">
                        <img src="${item.image || 'https://via.placeholder.com/70'}" alt="${item.title || 'Sản phẩm'}">
                        <div>
                            <a href="#" class="product-title">${item.title || 'Sản phẩm'}</a>
                            <span class="product-price">${item.priceFormatted || priceNum.toLocaleString('vi-VN') + '₫'}</span>
                        </div>
                    </div>
                </td>
                <td class="col-quantity">
                    <input type="number" class="quantity-input" value="${qtyNum}" min="1" data-index="${index}">
                    <a href="#" class="remove-btn" data-index="${index}">Xóa</a>
                </td>
                <td class="col-total">${formattedItemTotal}</td>
            </tr>
        `;
    });

    if (tbody) {
        tbody.innerHTML = html;
    }

    let formattedSubtotal = subtotal.toLocaleString('vi-VN') + "₫";
    let formattedTotalVND = subtotal.toLocaleString('vi-VN') + " VND";

    if (subtotalRowSpan) subtotalRowSpan.innerText = formattedSubtotal;
    if (totalPriceSpan) totalPriceSpan.innerText = formattedTotalVND;

    // Thay đổi số lượng trực tiếp trên input
    const quantityInputs = document.querySelectorAll(".quantity-input");
    quantityInputs.forEach(input => {
        input.addEventListener("change", function () {
            let index = this.getAttribute("data-index");
            let newQty = parseInt(this.value);
            if (newQty < 1 || isNaN(newQty)) {
                newQty = 1;
            }
            cart[index].quantity = newQty;
            localStorage.setItem("cart", JSON.stringify(cart));
            location.reload();
        });
    });

    // Xóa sản phẩm
    const removeBtns = document.querySelectorAll(".remove-btn");
    removeBtns.forEach(btn => {
        btn.addEventListener("click", function (e) {
            e.preventDefault();
            let index = this.getAttribute("data-index");
            cart.splice(index, 1);
            localStorage.setItem("cart", JSON.stringify(cart));
            location.reload();
        });
    });

    // Xử lý nút Thanh toán (Chuyển sang Checkout)
    if (checkoutBtn) {
        checkoutBtn.disabled = false;
        checkoutBtn.style.backgroundColor = "#2563eb";
        checkoutBtn.style.cursor = "pointer";
        checkoutBtn.style.opacity = "1";

        checkoutBtn.addEventListener("click", function (e) {
            e.preventDefault();

            if (cart.length === 0) {
                alert("Giỏ hàng của bạn đang trống!");
                return;
            }

            // Đóng gói dữ liệu giỏ hàng vào localStorage
            const checkoutData = {
                items: cart,
                subtotal: subtotal,
                note: orderNote ? orderNote.value : "",
                createdAt: new Date().toISOString()
            };

            localStorage.setItem("checkoutData", JSON.stringify(checkoutData));

            // Chuyển hướng sang Checkout
            window.location.href = "/Home/Checkout";
        });
    }
});