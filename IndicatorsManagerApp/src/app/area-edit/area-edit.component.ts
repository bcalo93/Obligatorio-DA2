import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { AreaService } from 'src/services';
import { Area } from 'src/models';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';


@Component({
  selector: 'app-area-edit',
  templateUrl: './area-edit.component.html',
  styleUrls: ['./area-edit.component.css']
})
export class AreaEditComponent implements OnInit {
  isEdit: boolean;
  areaId: string;
  errorMessage: string;
  title: string;

  name = new FormControl('', [Validators.required]);
  dataSource = new FormControl('', [Validators.required]);

  area: Area;

  constructor(
    private areaService: AreaService,
    private location: Location,
    private currentRoute: ActivatedRoute,
    private router: Router) {}

  ngOnInit() {
    const id = this.currentRoute.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.loadAreaDetails(id);
      this.title = 'Area Details';
    } else {
      this.isEdit = false;
      this.title = 'Create User';
    }
  }

  save() {
    const newArea = new Area();
    newArea.name = this.name.value;
    newArea.dataSource = this.dataSource.value;

    this.areaService.addArea(newArea).subscribe(
      response => {
        this.areaId = response.id;
        this.clearErrorMsg();
      },
      error => {
        this.errorMessage = error;
        this.router.navigate(['/areas', 1]);
      }
    );
  }

  loadAreaDetails(id: string) {
    this.areaService.getArea(id)
      .subscribe(area => {
        this.area = area;
        this.name.setValue(this.area.name);
        this.dataSource.setValue(this.area.dataSource);
        this.clearErrorMsg();
    });
  }

  clearErrorMsg() {
    this.errorMessage = '';
  }

  goBack(): void {
    this.location.back();
  }
}
