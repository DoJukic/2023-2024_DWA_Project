class ScrollToTop extends HTMLElement {
    constructor() {
        super();

        this.overflowResizeObserver = null;
    }

    static targetClass = "sttRotateOnTopDistanceTrigger";

    static singleton = null;

    connectedCallback() {
        if(ScrollToTop.singleton != null){
            console.warn("Multiple scroll to top components detected, this is not supported and will likely break something.");
        }
        ScrollToTop.singleton = this;

        this.innerHTML =
            html`
            <flex-row class="fGrow"
                style="align-items: flex-end;">
                
                <img class="sttRotateOnTopDistance scrollToTopImage" src="../res/img/scroll_to_top.png"
                    style="width: 40px; height: 40px; pointer-events: auto; opacity: 0.75; cursor: pointer;">
                <stt-simple-spacer style="display:block; width:30px;"></stt-simple-spacer>
            </flex-row>
            <stt-simple-spacer style="display:block; height:20px;"></stt-simple-spacer>
        `;

        // If the page is too small there is no use for this button, so we simply hide it.
        var _this = this;

        $(window).on('resize', function(){
            _this.checkDisplay();
        });
        _this.overflowResizeObserver = new ResizeObserver((entries) => {
            _this.checkDisplay();
        });
        _this.overflowResizeObserver.observe(document.body);

        _this.checkDisplay(_this);

        // If we're already at the top, turn us into a scroll-to-bottom button.
        var targetElement = _this.getElementsByClassName("scrollToTopImage")[0];
        $(window).on('scroll', function(){
            _this.checkRotation(targetElement);
        });
        _this.checkRotation(targetElement);

        // The actual functionality is here
        $(targetElement).on("click", function(){
            if(!$(targetElement).hasClass(ScrollToTop.targetClass)){
                window.scrollTo({ top: 0, behavior: 'smooth' });
            }else{
                window.scrollTo({ top: document.body.scrollHeight, behavior: 'smooth' });
            }
        })
    }
    
    checkDisplay(){
        if (($("body").height() - 300) > $(window).height()) {
            $(this).css("display", "flex");
            return;
        }
        $(this).css("display", "none");
    }
    
    checkRotation(element){
        if (window.scrollY > 150) {
            element.classList.remove(ScrollToTop.targetClass)
            return;
        }
        element.classList.add(ScrollToTop.targetClass);
    }
}

// This also checks the current dom for the component and runs connectedCallback() if it finds it
customElements.define('scroll-to-top-component', ScrollToTop);

// If nothing is found, however, we assume the programmer is just being lazy and simply show ourselves in.
if(!Boolean(ScrollToTop.singleton)){
    // It is important that we add these safely, and not through innerHTML, as otherwise connectedCallback() will be reactivated for every component.
    var tgt = document.createElement("scroll-to-top-component");
    document.body.prepend(tgt);
}