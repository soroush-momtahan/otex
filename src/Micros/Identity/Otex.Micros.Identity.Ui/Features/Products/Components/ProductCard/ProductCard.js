// ۱. ایمپورت مستقیم از NPM! (بدون هیچ ابزار بیلد یا node_modules)
import confetti from 'https://esm.sh/canvas-confetti@1.9.2';

// ۲. ساخت کلاس که رویای تو بود
class ProductCardController {

    // پراپرتی‌های کلاس
    vm; // ViewModel که از سرور میاد
    isHovered = false;

    // ۳. کانستراکتور دقیقاً ViewModel C# رو دریافت می‌کنه
    constructor(serverViewModel) {
        this.vm = serverViewModel;
    }

    // این متد پیش‌فرض Alpine است که بعد از ساخت کلاس اجرا میشه (مثل OnInitialized)
    init() {
        console.log(`کلاس برای محصول ${this.vm.Name} ساخته و آماده شد.`);
    }

    // متدهای تجاری و ظاهری
    toggleLike() {
        // تغییر وضعیت مدل
        this.vm.IsLiked = !this.vm.IsLiked;

        // استفاده از پکیج NPM
        if (this.vm.IsLiked) {
            confetti({
                particleCount: 100,
                spread: 70,
                origin: { y: 0.6 }
            });
        }
    }

    // یک Getter برای استفاده تمیز در HTML
    get buttonIcon() {
        return this.vm.IsLiked ? '❤️' : '🤍';
    }
}

// ۴. اتصال کلاس به Alpine.js
// این کد تضمین میکنه که Alpine این کلاس رو با اسم 'ProductCard' بشناسه
document.addEventListener('alpine:init', () => {
    Alpine.data('ProductCard', (serverViewModel) => new ProductCardController(serverViewModel));
});