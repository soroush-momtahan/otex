function registerSignOn() {
    Alpine.data('SignOn', () => ({
        
        init() {
        },
    }));
}
function registerOtpVerify() {
    Alpine.data('OtpVerify', () => ({
        digits: ['', '', '', '', '', ''],
        timeLeft: 120,
        timerInterval: null,

        init() {
            this.startTimer();
            this.$nextTick(() => {
                this.focusInput(0);
            });
        },

        focusInput(index) {
            const inputs = this.$root.querySelectorAll('input[inputmode="numeric"]');
            if (inputs[index]) {
                inputs[index].focus();
            }
        },
        startTimer() {
            this.timerInterval = setInterval(() => {
                if (this.timeLeft > 0) {
                    this.timeLeft--;
                } else {
                    clearInterval(this.timerInterval);
                }
            }, 1000);
        },

        get formattedTime() {
            const m = String(Math.floor(this.timeLeft / 60)).padStart(2, '0');
            const s = String(this.timeLeft % 60).padStart(2, '0');
            return `${m}:${s}`;
        },

        // 💡 متد جدید برای نوار پیشرفت (Progress Bar)
        get progressPercentage() {
            // ۱۲۰ ثانیه معادل ۱۰۰ درصد
            return (this.timeLeft / 120) * 100;
        },

        handleInput(index, event) {
            const val = event.target.value;
            if (!/^[0-9]$/.test(val)) {
                this.digits[index] = '';
                return;
            }

            if (val && index < 5) {
                this.focusInput(index + 1);
            }

            if (this.digits.every(d => d !== '')) {
                // اضافه کردن تاخیر کمی بیشتر برای اینکه انیمیشن پر شدن باکس آخر دیده شود
                setTimeout(() => {
                    this.$refs.autoSubmitBtn.click();
                }, 300);
            }
        },

        handleBackspace(index, event) {
            if (!this.digits[index] && index > 0) {
                this.digits[index - 1] = '';
                this.focusInput(index - 1);
            }
        },

        handlePaste(event) {
            event.preventDefault();
            const pastedData = event.clipboardData.getData('text').slice(0, 6).split('');
            if (pastedData.length > 0 && pastedData.every(char => /^[0-9]$/.test(char))) {
                for (let i = 0; i < pastedData.length; i++) {
                    this.digits[i] = pastedData[i];
                }
                const focusIndex = pastedData.length < 6 ? pastedData.length : 5;
                this.focusInput(focusIndex);

                if (this.digits.every(d => d !== '')) {
                    setTimeout(() => this.$refs.autoSubmitBtn.click(), 300);
                }
            }
        },
    }));
}

// حل مشکل زمان‌بندی لود فایل (مخصوص کار با Hydro و لودهای داینامیک)
if (window.Alpine) {
    registerSignOn();
    // اگر آلپاین از قبل لود شده (مثلا کامپوننت داینامیک اضافه شده) مستقیم ثبتش کن
    registerOtpVerify();
} else {
    document.addEventListener('alpine:init', registerSignOn);
    // اگر بارگذاری اولیه صفحه است، منتظر رویداد init بمان
    document.addEventListener('alpine:init', registerOtpVerify);
}
