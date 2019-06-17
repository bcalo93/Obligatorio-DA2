import { Component, OnInit } from '@angular/core';
import { Area } from 'src/models';
import { AreaService } from 'src/services';

@Component({
  selector: 'app-area-list',
  templateUrl: './area-list.component.html',
  styleUrls: ['./area-list.component.css']
})
export class AreaListComponent implements OnInit {

  areas: Array<Area>;
  errorMessage: string;

  constructor(private areaService: AreaService) { }

  ngOnInit() {
    this.areaService.getAllAreas()
      .subscribe(
        areas => this.areas = areas,
        error => this.errorMessage = error
      );
  }

  deleteArea(id: string) {
    this.areaService.deleteArea(id).subscribe(
      () => {
        const index = this.areas.findIndex(x => x.id === id);
        this.areas.splice(index, 1);
        console.log('Area deleted');
      },
      error => this.errorMessage = error
      );
  }

}
