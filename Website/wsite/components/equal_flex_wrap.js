class equalFlexWrap extends HTMLElement {
    constructor() {
        super();
    }

    static overflowResizeObserver = null;

    minElementSizePx = 200;
    elementGapPx = 20;

    connectedCallback() {
        let temp = ML.getIntAttribute(this, "min-element-size-px");
        if(Boolean(temp)){
            this.minElementSizePx = temp;
        }

        temp = ML.getIntAttribute(this, "element-gap-px");
        if(temp == 0 || Boolean(temp)){
            this.elementGapPx = temp;
        }
        
        /* We'll want to avoid destroying everything in style while changing things in this case */
        this.style.setProperty("--splitter-gap", `${this.elementGapPx}px`);
        this.style.gap = `${this.elementGapPx}px`;

        // Fires on any resize but is always a bit late
        equalFlexWrap.overflowResizeObserver.observe(this);
        // Fires only on window resize but is never late
        //addEventListener("resize", (event) => {this.balanceDisplay();});
        this.balanceDisplay();
    }

    balanceDisplay() {
        let elementSizePx = 0;
        this.childrenAmountPerRow = this.children.length

        let availableWidth = $(this).width();

        if (availableWidth < 1)
            availableWidth = 2000; // WELP 2: Electric Boogaloo
            // (This would cause an infinite loop if for example our display is set to none)

        let neededSize = this.childrenAmountPerRow * this.minElementSizePx;
        let diff = neededSize / availableWidth;

        while(diff > 1){
            diff = diff / 2;
            this.childrenAmountPerRow = this.childrenAmountPerRow / 2;
        }
        
        this.childrenAmountPerRow = Math.floor(this.childrenAmountPerRow);

        if (this.childrenAmountPerRow <= 0) // WELP
            this.childrenAmountPerRow = 1;

        elementSizePx = availableWidth / this.childrenAmountPerRow - 10.0 / this.childrenAmountPerRow;

        // The -5 gives us some breathing room - as we're tracking resire asynchronously we might sometimes be a bit behind
        
        this.style.setProperty("--splitter-basis", `${elementSizePx}px`);
        this.style.setProperty("--splitter-elems-per-line", `${this.childrenAmountPerRow}`);
    }
}

equalFlexWrap.overflowResizeObserver =  new ResizeObserver((entries) => {
    entries.forEach((entry) => {
        entry.target.balanceDisplay();
    });
});

customElements.define('flex-row-eq-wrap-component', equalFlexWrap);