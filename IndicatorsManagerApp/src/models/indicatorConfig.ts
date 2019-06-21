const MAX_INT =  2147483647;

export class IndicatorConfig {
    indicatorId: string;
    position: number;
    isVisible: boolean;
    alias: string;

    constructor(indicatorConfig: IndicatorConfig) {
      this.indicatorId = indicatorConfig.indicatorId;
      this.isVisible = indicatorConfig.isVisible;
      this.position = (typeof indicatorConfig.position !== 'undefined') ? indicatorConfig.position : MAX_INT;
      this.alias = indicatorConfig.alias;
    }

    setPosition(position: number) {
      this.position = position;
    }

    toggleVisibility() {
      this.isVisible = !this.isVisible;
    }
}
