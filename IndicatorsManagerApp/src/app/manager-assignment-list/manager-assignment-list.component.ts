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
        this.assignedManagers = [...area.users];
        this.currentAssignedManagers = [...area.users];
        this.userService.getAllUsers()
        .subscribe(
          users => {
            const managers = users.filter(user => user.role === UserRole.MANAGER);
            if (this.currentAssignedManagers.length > 0) {
              const unassignedManagers = managers.filter(manager => !this.userIsAlreadyAssigned(manager));
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


  drop(event: CdkDragDrop<Array<User>>) {
    const managers = event.previousContainer.data;
    const actualManager = managers[event.previousIndex];
    if (event.previousContainer !== event.container) {
      this.areaService.addManagerToArea(this.areaId, actualManager.id)
      .subscribe(
        response => {
          this.currentAssignedManagers = [...response.users]
          transferArrayItem(event.previousContainer.data,
            event.container.data,
            event.previousIndex,
            event.currentIndex);
        },
        error => console.log(error)
      );
    }
  }

  private userIsAlreadyAssigned(user: User): boolean {
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
            this.areaService.deleteManagerFromArea(this.areaId, item.id).subscribe(
              () => {
                this.assignedManagers = [];
                this.currentAssignedManagers = [];
              },
              error => this.errorMessage = error
            )
          );
      }
    });
  }

  removeManager(manager: User) {
    this.areaService.deleteManagerFromArea(this.areaId, manager.id).subscribe(
      () => {
        this.allManagers.push(manager);
        const aux = [...this.currentAssignedManagers];
        const indexDeletedManager = aux.findIndex(x => x.id === manager.id);
        aux.splice(indexDeletedManager, 1);
        this.assignedManagers = [...aux];
        this.currentAssignedManagers = [...aux];
      },
      error => this.errorMessage = error
    );
  }


  goBack() {
    const backUrl = this.router.url.split('/').slice(1,3).join('/')
    this.router.navigate([backUrl]);
  }
}
