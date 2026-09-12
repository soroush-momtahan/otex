// import {default as Utils} from "./Services/OtpVerifyUtills.js";
// import {default as Transformer} from "./Services/OtpVerifyTransformer.js"
//
// export function registerOtpVerify() {
//     Alpine.data('OtpVerify', () => ({
//         digits: ['', '', '', '', '', ''],
//         timeLeft: 120,
//         timerInterval: null,
//         hasSubmitted: false,
//
//         init() {
//             // this.$nextTick(() => {
//             //     const initialOtp = this.$refs.hiddenOtp ? this.$refs.hiddenOtp.value : '';
//             //     if (initialOtp && initialOtp.length === 6) {
//             //         this.digits = initialOtp.split('');
//             //     } else {
//             //         this.focusInputTransformer(0);
//             //     }
//             // });
//             Transformer.focusInput(this.$root, 0);
//             this.startTimer();
//         },
//
//         startTimer() {
//             this.timerInterval = setInterval(() => {
//                 if (this.timeLeft > 0) {
//                     this.timeLeft--;
//                 } else {
//                     clearInterval(this.timerInterval);
//                 }
//             }, 1000);
//         },
//
//         resetOtp() {
//             this.hasSubmitted = false;
//             this.digits = ['', '', '', '', '', ''];
//             Transformer.focusInput(this.$root, 0);
//         },
//
//         get formattedTime() {
//             const m = String(Math.floor(this.timeLeft / 60)).padStart(2, '0');
//             const s = String(this.timeLeft % 60).padStart(2, '0');
//             return `${m}:${s}`;
//         },
//
//         get progressPercentage() {
//             return (this.timeLeft / 120) * 100;
//         },
//
//         onInput(index, event) {
//             const val = event.target.value;
//
//             if (!Utils.isDigitInput(val)) {
//                 this.digits[index] = '';
//                 return;
//             }
//
//             this.digits[index] = val;
//
//             if (val && index < 5) {
//                 Transformer.focusInput(this.$root, index + 1);
//             }
//
//             if (Utils.everyAreDigits(this.digits)) {
//                 setTimeout(() => {
//                     this.onSubmitForm();
//                 }, 300)
//             }
//         },
//
//         onBackspace(index, event) {
//             if (!this.digits[index] && index > 0) {
//                 this.digits[index - 1] = '';
//                 Transformer.focusInput(this.$root, index - 1);
//             }
//         },
//
//         onPaste(event) {
//             event.preventDefault();
//
//             const pastedData = event.clipboardData.getData('text').slice(0, 6).split('');
//             if (pastedData.length === 6 && Utils.everyAreDigits(pastedData)) {
//                 for (let i = 0; i < pastedData.length; i++) {
//                     this.digits[i] = pastedData[i];
//                 }
//                 const focusIndex = pastedData.length < 6 ? pastedData.length : 5;
//                 Transformer.focusInput(this.$root, focusIndex);
//
//                 this.onSubmitForm();
//             } else {
//                 const focusIndex = pastedData.length < 6 ? pastedData.length : 5;
//                 Transformer.focusInput(this.$root, focusIndex);
//             }
//         },
//
//         onSubmitForm() {
//             this.hasSubmitted = true;
//             Transformer.triggerAutoSubmit(this.$refs, this.digits.join(''));
//             this.resetOtp();
//         },
//     }));
// }

import {default as Utils} from "./Services/OtpVerifyUtills.ts"; // حذف پسوند js
import Transformer from "./Services/OtpVerifyTransformer";
import type {Alpine} from 'alpinejs'; // ایمپورت تایپ آلپاین

// نام تابع را به register تغییر دادیم تا سیستم Auto-Discovery در main.ts کار کند
export function register(Alpine: Alpine) {
    Alpine.data('OtpVerify', () => ({
        // 1. معرفی دقیق تایپ متغیرها
        digits: ['', '', '', '', '', ''] as string[],
        timeLeft: 120,
        timerInterval: null as ReturnType<typeof setInterval> | null,
        hasSubmitted: false,

        init() {
            // به تایپ اسکریپت میگیم این $root یک المنت HTML است
            Transformer.focusInput(this.$root as HTMLElement, 0);
            this.startTimer();
        },

        startTimer() {
            this.timerInterval = setInterval(() => {
                if (this.timeLeft > 0) {
                    this.timeLeft--;
                } else {
                    if (this.timerInterval) {
                        clearInterval(this.timerInterval);
                    }
                }
            }, 1000);
        },

        resetOtp() {
            this.hasSubmitted = false;
            this.digits = ['', '', '', '', '', ''];
            Transformer.focusInput(this.$root as HTMLElement, 0);
        },

        // 2. خروجی متدها مشخص شده
        get formattedTime(): string {
            const m = String(Math.floor(this.timeLeft / 60)).padStart(2, '0');
            const s = String(this.timeLeft % 60).padStart(2, '0');
            return `${m}:${s}`;
        },

        get progressPercentage(): number {
            return (this.timeLeft / 120) * 100;
        },

        // 3. تایپ دادن به پارامترهای Event
        onInput(index: number, event: Event) {
            // کست کردن تارگت به Input برای دسترسی بدون ارور به value
            const target = event.target as HTMLInputElement;
            const val = target.value;

            if (!Utils.isDigitInput(val)) {
                this.digits[index] = '';
                return;
            }

            this.digits[index] = val;

            if (val && index < 5) {
                Transformer.focusInput(this.$root as HTMLElement, index + 1);
            }

            if (Utils.everyAreDigits(this.digits)) {
                setTimeout(() => {
                    this.onSubmitForm();
                }, 300)
            }
        },

        onBackspace(index: number, event: KeyboardEvent) {
            if (!this.digits[index] && index > 0) {
                this.digits[index - 1] = '';
                Transformer.focusInput(this.$root as HTMLElement, index - 1);
            }
        },

        onPaste(event: ClipboardEvent) {
            event.preventDefault();

            // در تایپ اسکریپت clipboardData ممکن است null باشد، پس باید چک کنیم
            if (!event.clipboardData) return;

            const pastedData = event.clipboardData.getData('text').slice(0, 6).split('');

            if (pastedData.length === 6 && Utils.everyAreDigits(pastedData)) {
                for (let i = 0; i < pastedData.length; i++) {
                    this.digits[i] = pastedData[i];
                }
                const focusIndex = pastedData.length < 6 ? pastedData.length : 5;
                Transformer.focusInput(this.$root as HTMLElement, focusIndex);

                this.onSubmitForm();
            } else {
                const focusIndex = pastedData.length < 6 ? pastedData.length : 5;
                Transformer.focusInput(this.$root as HTMLElement, focusIndex);
            }
        },

        onSubmitForm() {
            this.hasSubmitted = true;
            // $refs را هم کست میکنیم تا Transformer ارور ندهد
            Transformer.triggerAutoSubmit(this.$refs as Record<string, HTMLElement>, this.digits.join(''));
            this.resetOtp();
        },
    }));
}