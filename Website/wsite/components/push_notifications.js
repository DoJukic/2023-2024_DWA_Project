class PushNotifs extends HTMLElement {
    constructor() {
        super();
    }

    lastAnimTimestamp = 0;

    static ready(){return(Boolean(PushNotifs.singleton))}

    static singleton = null;

    static pushNotifSuccessData = 
    html`
        <push-notification class="defaultMarginHalf roundedBorderHalf"
            style="margin-top: 0rem; border-top-left-radius: 0rem; border-bottom-left-radius: 0rem;">

            <push-notification-success class="centered">
                <div class="ui-icon ui-icon-check"></div>
            </push-notification-success>
            
            <push-notification-content class="defaultPaddingHalf">
    `

    static pushNotifInfoData = 
    html`
        <push-notification class="defaultMarginHalf roundedBorderHalf"
            style="margin-top: 0rem; border-top-left-radius: 0rem; border-bottom-left-radius: 0rem;">

            <push-notification-info class="centered">
                <div class="ui-icon ui-icon-info"></div>
            </push-notification-info>
            
            <push-notification-content class="defaultPaddingHalf">
    `

    static pushNotifFailData = 
    html`
        <push-notification class="defaultMarginHalf roundedBorderHalf"
            style="margin-top: 0rem; border-top-left-radius: 0rem; border-bottom-left-radius: 0rem;">

            <push-notification-fail class="centered">
                <div class="ui-icon ui-icon-alert"></div>
            </push-notification-fail>
            
            <push-notification-content class="defaultPaddingHalf">
    `

    static pushNotifEndData =
    html`
            </push-notification-content>
        </push-notification>
    `

    connectedCallback() {
        if(PushNotifs.singleton != null){
            console.warn("Multiple push notification components detected, this is not supported and will likely break something.");
        }
        PushNotifs.singleton = this;

        this.style.transform = "translateY(0px)"

        // https://stackoverflow.com/questions/6065169/requestanimationframe-with-this-keyword (thanks, James World!)
        requestAnimationFrame(() => this.animateNotifs());
    }

    static pushNotificationSuccess(title, content){
        var _this = PushNotifs.singleton;
        
        _this.insertAdjacentHTML("beforeend",
            PushNotifs.pushNotifSuccessData +
            "<text class='textSuccess'>" + title + "</text>" + "<br/><br-half></br-half>" + "<text>" + content + "</text>"
            + PushNotifs.pushNotifEndData);

        _this.notifAdded();
    }

    static pushNotificationInfo(title, content){
        var _this = PushNotifs.singleton;
        
        _this.insertAdjacentHTML("beforeend",
            PushNotifs.pushNotifInfoData +
            "<text class='textInfo'>" + title + "</text>" + "<br/><br-half></br-half>" + "<text>" + content + "</text>"
            + PushNotifs.pushNotifEndData);

        _this.notifAdded();
    }

    static pushNotificationFail(title, content){
        var _this = PushNotifs.singleton;
        
        _this.insertAdjacentHTML("beforeend",
            PushNotifs.pushNotifFailData +
            "<text class='textError''>" + title + "</text>" + "<br/><br-half></br-half>" + "<text>" + content + "</text>"
            + PushNotifs.pushNotifEndData);

        _this.notifAdded();
    }

    notifAdded(){
        var target = this.children[this.children.length - 1];
        var height = $(target).outerHeight(true);

        var transformValue = Number(this.style.transform.split("(")[1].split("px")[0]);
        this.style.transform = `translateY(${transformValue + height}px)`;

        // Not sure why none of the animation() methods want to work right.
        // UPDATE: I was adding to innerHTML directly, which broke the references.
        
        $(target).delay(4000).animate({
            opacity: 0
            }, 500, function(){
                target.remove();
            }
        );
    }

    animateNotifs(){
        var transformValue = Number(this.style.transform.split("(")[1].split("px")[0]);

        if(transformValue > 0){
            var deltaTime = window.performance.now() - this.lastAnimTimestamp;
            transformValue -= (Number)(deltaTime) / 200 * transformValue; // Ease out, essentially
            this.style.transform = `translateY(${(transformValue >= 1) ? transformValue : 0}px)`;
        }

        this.lastAnimTimestamp = window.performance.now();

        requestAnimationFrame(() => this.animateNotifs()); // It's probably fine if we just keep running
    }
}

customElements.define('push-notifications-component', PushNotifs);

if(!Boolean(PushNotifs.singleton)){
    var tgt = document.createElement("push-notifications-component");
    document.body.prepend(tgt);
}

ML.subscribeToAccessTokenExpired(() => {PushNotifs.pushNotificationInfo("TOKEN EXPIRED", "Your access token has expired, please log in again.")});
ML.subscribeToUserThemeChange(() => {PushNotifs.pushNotificationInfo("THEME CHANGED", `Your theme has been set to [${ML.getCurrentTheme()}].`)});