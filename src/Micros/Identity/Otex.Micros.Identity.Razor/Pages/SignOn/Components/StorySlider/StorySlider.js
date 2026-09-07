// class StorySlider {
//     // پراپرتی‌ها
//     Slides = [];
//     currentIndex = 0;
//     slideDuration = 4000;
//     timer = null;
//
//     // گرفتن دیتای C#
//     constructor(viewModel) {
//         // چون PropertyNamingPolicy = null تنظیم شده، نام‌ها با حروف بزرگ شروع می‌شوند
//         this.Slides = viewModel.Slides;
//         console.log("Helllllloooooooo")
//     }
//
//     // این متد پیش‌فرض Alpine به محض لود شدن اجرا می‌شود
//     init() {
//         // تغییر رنگ پس‌زمینه اصلی (main-bg) با استفاده از قابلیت $watch در Alpine
//         // هر زمان currentIndex عوض شود، این تابع خودکار اجرا می‌شود
//         this.$watch('currentIndex', () => this.updateBodyBg());
//         this.updateBodyBg(); // برای بار اول
//
//         this.startTimer();
//     }
//
//     startTimer() {
//         this.timer = setInterval(() => {
//             this.currentIndex = (this.currentIndex + 1) % this.Slides.length;
//         }, this.slideDuration);
//     }
//
//     // آپدیت کردن بک‌گراند خارج از کامپوننت (اگر body-bg خارج از این HTML است)
//     updateBodyBg() {
//         const bodyBg = document.getElementById('main-bg');
//         if (bodyBg) {
//             // حذف تمام رنگ‌های احتمالی
//             this.Slides.forEach(s => bodyBg.classList.remove(s.BgColor));
//             // اضافه کردن رنگ اسلاید فعلی
//             bodyBg.classList.add(this.currentSlide.BgColor);
//         }
//     }
//
//     // یک Getter برای دسترسی راحت به اسلاید فعلی در HTML
//     get currentSlide() {
//         return this.Slides[this.currentIndex];
//     }
// }
//
// document.addEventListener('alpine:init', () => {
//     Alpine.data('StorySlider', (storySliderViewModel) => new StorySlider(storySliderViewModel));
// });

// تعریف ماژول به صورت یک آبجکت استاندارد Alpine
function registerStorySlider() {
    Alpine.data('StorySlider', (viewModel) => ({
        Slides: viewModel.Slides || [],
        currentIndex: 0,
        slideDuration: 4000,
        timer: null,
        
        // این متد پیش‌فرض Alpine به محض لود شدن اجرا می‌شود
        init() {
            this.$watch('currentIndex', () => this.updateBodyBg());
            this.updateBodyBg(); // برای بار اول
            this.startTimer();
        },

        startTimer() {
            this.timer = setInterval(() => {
                this.currentIndex = (this.currentIndex + 1) % this.Slides.length;
            }, this.slideDuration);
        },

        updateBodyBg() {
            const bodyBg = document.getElementById('main-bg');
            if (bodyBg) {
                // حذف تمام رنگ‌های احتمالی
                this.Slides.forEach(s => bodyBg.classList.remove(s.BgColor));

                // اضافه کردن رنگ اسلاید فعلی (بررسی وجود برای جلوگیری از خطا)
                if(this.currentSlide) {
                    bodyBg.classList.add(this.currentSlide.BgColor);
                }
            }
        },

        // یک Getter برای دسترسی راحت به اسلاید فعلی در HTML
        get currentSlide() {
            return this.Slides[this.currentIndex];
        }
    }));
}

// حل مشکل زمان‌بندی لود فایل (مخصوص کار با Hydro و لودهای داینامیک)
if (window.Alpine) {
    // اگر آلپاین از قبل لود شده (مثلا کامپوننت داینامیک اضافه شده) مستقیم ثبتش کن
    registerStorySlider();
} else {
    // اگر بارگذاری اولیه صفحه است، منتظر رویداد init بمان
    document.addEventListener('alpine:init', registerStorySlider);
}