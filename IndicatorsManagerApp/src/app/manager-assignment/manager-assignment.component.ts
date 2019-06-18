import { Component, OnInit } from '@angular/core';
import { AreaService } from 'src/services';
import { User, Area } from 'src/models';
import { Router } from '@angular/router';

@Component({
  selector: 'app-manager-assignment',
  templateUrl: './manager-assignment.component.html',
  styleUrls: ['./manager-assignment.component.css']
})
export class ManagerAssignmentComponent implements OnInit {

  allAreas: Array<Area>;
  selectedArea: Area;

  allManagers: Array<User>;

  assignedManagers: Array<User>;

  errorMessage = '';

  constructor(private areaService: AreaService, private router: Router) { }

  ngOnInit() {
    this.areaService.getAllAreas().subscribe(
      areas => this.allAreas = areas.reverse(),
      error => this.errorMessage = error
    );
  }

  areaSelection() {
    this.router.navigate(['areas/managers', this.selectedArea]);
  }
}
