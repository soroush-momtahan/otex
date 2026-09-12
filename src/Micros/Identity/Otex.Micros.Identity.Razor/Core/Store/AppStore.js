function appStore() {
    Alpine.store('appStore', ({
        verifyOtp(code) {
            console.log("Sending command to backend...", code);
            // Http call...
        }
    }));
}

// if (window.Alpine){
//     appStore();
// }
// else {
//     document.addEventListener('alpine:init', () => appStore);
// }

appStore();