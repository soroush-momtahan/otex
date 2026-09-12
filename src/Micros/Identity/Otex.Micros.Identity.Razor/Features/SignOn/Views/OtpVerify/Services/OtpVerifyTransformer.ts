// export default class OtpVerifyTransformer {
//     static focusInput(rootElement, index) {
//         const inputs = rootElement.querySelectorAll('input[inputmode="numeric"]');
//         if (inputs[index]) {
//             inputs[index].focus();
//         }
//     }
//     static triggerAutoSubmit($refs, finalOtpCode){
//         $refs.hiddenOtp.value = finalOtpCode;
//         $refs.hiddenOtp.dispatchEvent(new Event('change', {bubbles: true}));
//         $refs.autoSubmitBtn.click();
//     }
// }

export default class OtpVerifyTransformer {
    static focusInput(rootElement: HTMLElement, index: number): void {
        // Typecasting for safety
        const inputs = Array.from(rootElement.querySelectorAll<HTMLInputElement>('input[inputmode="numeric"]'));
        if (inputs[index]) {
            inputs[index].focus();
        }
    }
    static triggerAutoSubmit(refs: Record<string, HTMLElement>, finalOtpCode: string): void {
        const hiddenOtp = refs.hiddenOtp as HTMLInputElement;
        const autoSubmitBtn = refs.autoSubmitBtn as HTMLButtonElement;

        hiddenOtp.value = finalOtpCode;
        hiddenOtp.dispatchEvent(new Event('change', { bubbles: true }));
        autoSubmitBtn.click();
    }
}