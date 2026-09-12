// export default class OtpVerifyUtils {
//     static isDigitInput(val) {
//         return /^[0-9]$/.test(val);
//     } 
//     static everyAreDigits(digits) {
//         return !!digits.every(d => d !== '' && /^[0-9]$/.test(d));
//     }
//    
// }

export default class OtpVerifyUtils {
    static isDigitInput(val: string): boolean {
        return /^[0-9]$/.test(val);
    }
    static everyAreDigits(digits: string[]): boolean {
        return digits.every(d => d !== '' && /^[0-9]$/.test(d));
    }
}