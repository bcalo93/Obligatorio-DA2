import { Component, OnInit, Input } from '@angular/core';
import { AreaService, UserService, AuthService } from 'src/services';
import { User, Area } from 'src/models';
import { UserRole } from 'src/enums';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { ActivatedRoute, Router } from '@angular/router';
import { DialogComponent } from '../dialog/dialog.component';
import { MatDialog } from '@angular/material';

@Component({
  selector: 'app-manager-assignment-list',
  templateUrl: './manager-assignment-list.component.html',
  styleUrls: ['./manager-assignment-list.component.css']
})
export class ManagerAssignmentListComponent implements OnInit {

  areaId: string;
  allManagers: Array<User>;
  assignedManagers: Array<User>;
  errorMessage = '';
  areaSelected = new Area();

  currentAssignedManagers = new Array<User>();

  constructor(
    private authService: AuthService,
    private areaService: AreaService,
    private userService: UserService,
    private currentRoute: ActivatedRoute,
    private router: Router,
    public dialog: MatDialog) { }

  ngOnInit() {
    const id = this.currentRoute.snapshot.paramMap.get('id');
    this.areaId = id;
    this.areaService.getArea(this.areaId).subscribe(
      response => {
        const area = response;
        this.areaSelected = area;
        console.log(area);
        this.assignedManagers = [...area.users];
        this.currentAssignedManagers = [...area.users];
        console.log('CURRENT ASSIGNED MANAGERS', this.currentAssignedManagers)
        this.userService.getAllUsers().subscribe(
        users => {
          const managers = users.filter(user => user.role === UserRole.MANAGER);
          if (this.currentAssignedManagers.length > 0) {
            const unassignedManagers = managers.filter(manager => !this.userIsAlreadyAssigned(manager));
            console.log('UNASSIGNED MANAGERS',unassignedManagers);
            this.allManagers = [...unassignedManagers];
          } else {
            this.allManagers = [...managers];
          }
        },
        error => this.errorMessage = error
        );
      },
      error => this.errorMessage = error
    );
  }


  drop(event: CdkDragDrop<Array<Area>>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(event.previousContainer.data,
                        event.container.data,
                        event.previousIndex,
                        event.currentIndex);
    }
  }

  updateArea() {
    console.log(this.assignedManagers);
    if (this.assignedManagers.length === 0) {
      this.errorMessage = 'Drag & Drop some manager to the asigned manager area';
    } else {
      console.log('ASSIGNED USERS',this.assignedManagers)
      this.assignedManagers.forEach(user => {
          console.log('EACH USER',user)
          console.log('CURRENT ASSIGNED MANAGERS',this.currentAssignedManagers)
          console.log(!this.currentAssignedManagers.some(x => x.id === user.id))
          if (!this.userIsAlreadyAssigned(user)) {
            this.areaService.addManagerToArea(this.areaId, user.id).subscribe(
              response => console.log(response),
              error => console.log(error)
            );
          }
        });
      }
  }

  private userIsAlreadyAssigned(user: User): boolean{
    return this.currentAssignedManagers.some(x => x.id === user.id);
  }

  removeAll() {
    const dialogRef = this.dialog.open(DialogComponent, {
      width: '250px',
      data: {
        message: 'Are you sure you want to unassign all managers from this area?',
        currentUser: this.authService.getCurrentUser()
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
          this.allManagers.push(...this.assignedManagers);
          this.assignedManagers.forEach(item =>
            this.areaService.deleteManagerFromArea(this.areaId, item.id).subscribe()
          );
          this.assignedManagers = [];
      }
    });
  }

  removeManager(){}


  goBack() {
    const backUrl = this.router.url.split('/').slice(1,3).join('/')
    this.router.navigate([backUrl]);
  }
}
