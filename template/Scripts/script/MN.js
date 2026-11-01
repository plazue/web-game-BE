
    (function () {
            const slides = Array.from(document.querySelectorAll('.hero-slide'));
    const dots = Array.from(document.querySelectorAll('.hero-dot'));
    const pauseButton = document.getElementById('heroPause');
    if (!slides.length) return;

    let current = 0;
    let paused = false;
    let timer;

    function showSlide(index) {
        current = (index + slides.length) % slides.length;
                slides.forEach((slide, i) => slide.classList.toggle('active', i === current));
                dots.forEach((dot, i) => dot.classList.toggle('active', i === current));
            }

    function startTimer() {
        clearInterval(timer);
    if (!paused) {
        timer = setInterval(() => showSlide(current + 1), 5000);
                }
            }

            dots.forEach(dot => {
        dot.addEventListener('click', () => {
            showSlide(Number(dot.dataset.slide));
            startTimer();
        });
            });

            pauseButton?.addEventListener('click', () => {
        paused = !paused;
    pauseButton.innerHTML = paused
    ? '<i class="fa-solid fa-play"></i>'
    : '<i class="fa-solid fa-pause"></i>';
    startTimer();
            });

    showSlide(0);
    startTimer();
        })();

    // TÌM NHANH SẢN PHẨM TRONG PHẦN HOME
    (function () {
            const input = document.getElementById('productSearch');
    const cards = Array.from(document.querySelectorAll('#productGrid .product-card'));
    if (!input || !cards.length) return;

    input.addEventListener('input', function () {
                const keyword = this.value.trim().toLowerCase();
                cards.forEach(card => {
                    const name = (card.dataset.name || '').toLowerCase();
    card.style.display = !keyword || name.includes(keyword) ? '' : 'none';
                });
            });
        })();