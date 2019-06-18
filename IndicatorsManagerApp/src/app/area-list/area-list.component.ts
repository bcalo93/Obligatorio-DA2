import { Component, OnInit } from '@angular/core';
import { Area } from 'src/models';
import { AreaService } from 'src/services';
import { Router } from '@angular/router';

@Component({
  selector: 'app-area-list',
  templateUrl: './area-list.component.html',
  styleUrls: ['./area-list.component.css']
})
export class AreaListComponent implements OnInit {

  areas: Array<Area>;
  errorMessage: string;

  constructor(private areaService: AreaService,
    private router: Router) { }

  ngOnInit() {
    this.areaService.getAllAreas()
      .subscribe(
        areas => this.areas = areas.reverse(),
        error => this.errorMessage = error
      );
  }
  editArea(id: string){
    this.router.navigate(['areas', id]);
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
