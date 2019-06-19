export class IndicatorConfig {
    indicatorId: string;
    position: number;
    isVisible: boolean;
    alias: string;

    constructor(indicatorConfig: IndicatorConfig) {
      this.indicatorId = indicatorConfig.indicatorId;
      this.isVisible = indicatorConfig.isVisible;
      this.position = (indicatorConfig.position) ? indicatorConfig.position : 0;
      this.alias = indicatorConfig.alias;
    }

    setPosition(position: number) {
      this.position = position;
    }

    toggleVisibility() {
      this.isVisible = !this.isVisible;
    }
}
