document.addEventListener("DOMContentLoaded", () => {
    const slides = [
        { bgColor: "bg-violet-600", image: "/images/slide-00.png", title: "خرید لذت بخش لنت", desc: "با چند لمس لنت خود را سفارش دهید و درب منزل تحویل بگیرید" },
        { bgColor: "bg-indigo-600", image: "/images/slide-04.png", title: "خرید عمده، سود بیشتر", desc: "به خانواده اُتکس بپیوندید. لیست قیمت فروش عمده را دریافت کرده و از شرایط ویژه و تخفیف‌های اختصاصی بهره‌مند شوید." },
        { bgColor: "bg-emerald-500", image: "/images/slide-01.png", title: "100 درصد بدون آزبست", desc: "محافظت از سلامتی شما و محیط زیست بخاطر فرمولاسیون ایمن و نداشتن مواد سمی" },
        { bgColor: "bg-blue-600", image: "/images/slide-02.png", title: "سکوت مطلق در هر ترمز", desc: "100 درصد بی صدا ، با شیم ضد صدا" },
        { bgColor: "bg-yellow-600", image: "/images/slide-03.png", title: "دارای گرید A+", desc: "بالاترین کیفیت ترمز گیری در کنار بالاترین درجه کیفیت" }
    ];

    const slideDuration = 4000;
    let currentIndex = 0;

    const bodyBg = document.getElementById('main-bg');
    const progressContainer = document.getElementById('progress-container');
    const slideContent = document.getElementById('slide-content');
    const imgEl = document.getElementById('slide-image');
    const titleEl = document.getElementById('slide-title');
    const descEl = document.getElementById('slide-desc');

    function buildProgressBars() {
        progressContainer.innerHTML = '';
        slides.forEach((_, index) => {
            const barWrapper = document.createElement('div');
            barWrapper.className = 'h-1 flex-1 bg-white/30 rounded-full overflow-hidden';
            const barFill = document.createElement('div');
            barFill.id = `bar-${index}`;
            barFill.className = 'h-full bg-white w-0 rounded-full';
            barWrapper.appendChild(barFill);
            progressContainer.appendChild(barWrapper);
        });
    }

    function updateSlide(index) {
        const slide = slides[index];
        slides.forEach(s => bodyBg.classList.remove(s.bgColor));
        bodyBg.classList.add(slide.bgColor);

        slideContent.classList.remove('animate-fade-in-up');
        void slideContent.offsetWidth;

        imgEl.src = slide.image;
        titleEl.textContent = slide.title;
        descEl.textContent = slide.desc;
        slideContent.classList.add('animate-fade-in-up');

        for (let i = 0; i < slides.length; i++) {
            const bar = document.getElementById(`bar-${i}`);
            bar.classList.remove('animate-story');

            if (i < index) {
                // بجای bar.style.width = '100%'
                bar.classList.remove('w-0');
                bar.classList.add('w-full');
            } else if (i === index) {
                // بجای bar.style.width = '0%'
                bar.classList.remove('w-full');
                bar.classList.add('w-0');

                void bar.offsetWidth; // Force Reflow
                bar.classList.add('animate-story');
            } else {
                // بجای bar.style.width = '0%'
                bar.classList.remove('w-full');
                bar.classList.add('w-0');
            }
        }
    }

    function autoHideErrors() {
        // پیدا کردن تمام المان هایی که کلاس ارور تیلویند را دارند
        const errorElements = document.querySelectorAll('.text-rose-500');

        if (errorElements.length > 0) {
            setTimeout(() => {
                errorElements.forEach(el => {
                    // اضافه کردن ترانزیشن نرم با کلاس های تیلویند
                    el.classList.add('transition-opacity', 'duration-500', 'opacity-0');

                    // بعد از اتمام انیمیشن، محتوای متن را پاک کن تا فضا را اشغال نکند
                    setTimeout(() => {
                        el.innerHTML = '';
                        el.classList.remove('opacity-0');
                    }, 500);
                });
            }, 4000); // 4 ثانیه مکث
        }
    }

    // فراخوانی در زمان لود اولیه صفحه
    autoHideErrors();

    // فراخوانی هر بار که HTMX فرم را آپدیت میکند
    document.body.addEventListener('htmx:afterSettle', function() {
        autoHideErrors();
    });

    function initErrorAnimation() {
        const errorContainer = document.getElementById('error-container');
        if (errorContainer && errorContainer.classList.contains('grid-rows-[1fr]')) {
            // 5 ثانیه ارور را نشان بده، بعد با انیمیشن ببندش
            setTimeout(() => {
                errorContainer.classList.remove('grid-rows-[1fr]', 'opacity-100', 'mb-4');
                errorContainer.classList.add('grid-rows-[0fr]', 'opacity-0', 'mb-0');
            }, 5000);
        }
    }

    // فراخوانی در زمان لود اولیه
    initErrorAnimation();
    // فراخوانی مجدد هر بار که HTMX فرم را آپدیت میکند
    document.body.addEventListener('htmx:afterSettle', function() {
        initErrorAnimation();
    });

    buildProgressBars();
    updateSlide(0);
    setInterval(() => {
        currentIndex = (currentIndex + 1) % slides.length;
        updateSlide(currentIndex);
    }, slideDuration);
});