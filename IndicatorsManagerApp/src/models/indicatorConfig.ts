export class IndicatorConfig {
    indicatorId: string;
    position: number;
    isVisible: boolean;
    alias: string;

    constructor(indicatorConfig: any) {
      this.indicatorId = indicatorConfig.id;
      if (indicatorConfig.alias) {
        this.alias = indicatorConfig.alias;
      } else { this.alias = indicatorConfig.name; }
      this.isVisible = indicatorConfig.isVisible;
      this.position = indicatorConfig.position;
    }

    setPosition(position: number){
      this.position = position;
    }

    toggleVisibility() {
      this.isVisible = !this.isVisible;
    }
}
