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
  buttonTitle: string;
  indicatorList: [];

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
      this.areaId = id;
      this.isEdit = true;
      this.loadAreaDetails(id);
      this.title = 'Area Details';
      this.buttonTitle = 'Save';
    } else {
      this.isEdit = false;
      this.title = 'Create area';
      this.buttonTitle = 'Create';
    }
  }

  save() {
    const newArea = new Area();
    newArea.name = this.name.value;
    newArea.dataSource = this.dataSource.value;
    if (!this.isEdit) {
      this.addArea(newArea);
    } else {
      newArea.id = this.areaId;
      this.updateArea(newArea);
    }
  }

  addArea(area: Area) {
    this.areaService.addArea(area).subscribe(
      response => {
        this.areaId = response.id;
        this.router.navigate(['/areas', this.areaId]);
        this.clearErrorMsg();
      },
      error => {
        this.errorMessage = error;
      }
    );
  }

  updateArea(area: Area) {
    this.areaService.updateArea(area)
    .subscribe(
      (data: Area) => {
        this.clearErrorMsg();
        console.log('Area Updated', data);
      },
      (error: any) => {
        this.errorMessage = error;
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

  isFormValid() {
    return !this.name.invalid && !this.dataSource.invalid;
  }
}
