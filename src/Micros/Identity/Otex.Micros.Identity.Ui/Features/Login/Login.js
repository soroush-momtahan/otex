// class Login {
//     digits = ['', '', '', '', '', ''];
//     timeLeft = 120;
//     timerInterval = null;
//     vm;
//
//     constructor(viewModel) {
//         this.vm = viewModel;
//     }
//
//     init() {
//         // شروع تایمر
//         this.startTimer();
//
//         // فوکوس اتوماتیک روی باکس اول بعد از لود صفحه
//         this.$nextTick(() => {
//             this.focusInput(0); // 👈 جایگزین شد
//         });
//     }
//
//     // 💡 متد جادویی جدید ما برای پیدا کردن و فوکوس روی اینپوت‌ها
//     focusInput(index) {
//         // تمام اینپوت‌های دارای inputmode="numeric" در این کامپوننت را پیدا می‌کنیم
//         const inputs = this.$root.querySelectorAll('input[inputmode="numeric"]');
//         if (inputs[index]) {
//             inputs[index].focus();
//         }
//     }
//
//     startTimer() {
//         this.timerInterval = setInterval(() => {
//             if (this.timeLeft > 0) {
//                 this.timeLeft--;
//             } else {
//                 clearInterval(this.timerInterval);
//             }
//         }, 1000);
//     }
//
//     get formattedTime() {
//         const m = String(Math.floor(this.timeLeft / 60)).padStart(2, '0');
//         const s = String(this.timeLeft % 60).padStart(2, '0');
//         return `${m}:${s}`;
//     }
//
//     handleInput(index, event) {
//         const val = event.target.value;
//         // فقط اعداد مجاز هستند
//         if (!/^[0-9]$/.test(val)) {
//             this.digits[index] = '';
//             return;
//         }
//
//         // پرش به باکس بعدی
//         if (val && index < 5) {
//             this.focusInput(index + 1); // 👈 جایگزین شد
//         }
//
//         // ارسال اتوماتیک
//         if (this.digits.every(d => d !== '')) {
//             setTimeout(() => {
//                 this.$refs.autoSubmitBtn.click();
//             }, 50);
//         }
//     }
//
//     handleBackspace(index, event) {
//         // اگر باکس خالی بود و بک‌اسپیس زده شد، برو به باکس قبلی
//         if (!this.digits[index] && index > 0) {
//             this.digits[index - 1] = '';
//             this.focusInput(index - 1); // 👈 جایگزین شد
//         }
//     }
//
//     handlePaste(event) {
//         event.preventDefault();
//         const pastedData = event.clipboardData.getData('text').slice(0, 6).split('');
//         if (pastedData.length > 0 && pastedData.every(char => /^[0-9]$/.test(char))) {
//             for (let i = 0; i < pastedData.length; i++) {
//                 this.digits[i] = pastedData[i];
//             }
//             // فوکوس روی آخرین باکس پر شده
//             const focusIndex = pastedData.length < 6 ? pastedData.length : 5;
//             this.focusInput(focusIndex); // 👈 جایگزین شد
//
//             // ارسال اتوماتیک در صورت کامل بودن
//             if (this.digits.every(d => d !== '')) {
//                 setTimeout(() => this.$refs.autoSubmitBtn.click(), 50);
//             }
//         }
//     }
// }
//
// document.addEventListener('alpine:init', () => {
//     Alpine.data('Login', (serverViewModel) => new Login(serverViewModel));
// });

class Login {
    digits = ['', '', '', '', '', ''];
    timeLeft = 120;
    timerInterval = null;
    vm;

    constructor(viewModel) {
        this.vm = viewModel;
    }

    init() {
        this.startTimer();
        this.$nextTick(() => {
            this.focusInputTransformer(0);
        });
    }

    focusInput(index) {
        const inputs = this.$root.querySelectorAll('input[inputmode="numeric"]');
        if (inputs[index]) {
            inputs[index].focus();
        }
    }
    startTimer() {
        this.timerInterval = setInterval(() => {
            if (this.timeLeft > 0) {
                this.timeLeft--;
            } else {
                clearInterval(this.timerInterval);
            }
        }, 1000);
    }

    get formattedTime() {
        const m = String(Math.floor(this.timeLeft / 60)).padStart(2, '0');
        const s = String(this.timeLeft % 60).padStart(2, '0');
        return `${m}:${s}`;
    }

    // 💡 متد جدید برای نوار پیشرفت (Progress Bar)
    get progressPercentage() {
        // ۱۲۰ ثانیه معادل ۱۰۰ درصد
        return (this.timeLeft / 120) * 100;
    }

    handleInput(index, event) {
        const val = event.target.value;
        if (!/^[0-9]$/.test(val)) {
            this.digits[index] = '';
            return;
        }

        if (val && index < 5) {
            this.focusInputTransformer(index + 1);
        }

        if (this.digits.every(d => d !== '')) {
            // اضافه کردن تاخیر کمی بیشتر برای اینکه انیمیشن پر شدن باکس آخر دیده شود
            setTimeout(() => {
                this.$refs.autoSubmitBtn.click();
            }, 300);
        }
    }

    handleBackspace(index, event) {
        if (!this.digits[index] && index > 0) {
            this.digits[index - 1] = '';
            this.focusInputTransformer(index - 1);
        }
    }

    handlePaste(event) {
        event.preventDefault();
        const pastedData = event.clipboardData.getData('text').slice(0, 6).split('');
        if (pastedData.length > 0 && pastedData.every(char => /^[0-9]$/.test(char))) {
            for (let i = 0; i < pastedData.length; i++) {
                this.digits[i] = pastedData[i];
            }
            const focusIndex = pastedData.length < 6 ? pastedData.length : 5;
            this.focusInputTransformer(focusIndex);

            if (this.digits.every(d => d !== '')) {
                setTimeout(() => this.$refs.autoSubmitBtn.click(), 300);
            }
        }
    }
}

document.addEventListener('alpine:init', () => {
    Alpine.data('Login', (serverViewModel) => new Login(serverViewModel));
});