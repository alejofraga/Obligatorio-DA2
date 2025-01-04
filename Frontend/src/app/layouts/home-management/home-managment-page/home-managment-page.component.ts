import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HomeService } from '../../../../backend/services/home.service';
import { AuthService } from '../../../../backend/services/auth.service';
import { HomeManagementService } from '../../../../backend/services/home-management.service';
import { LoadingService } from '../../../../backend/services/loading.service';

@Component({
  selector: 'app-home-managment-page',
  templateUrl: './home-managment-page.component.html',
})
export class HomeManagmentPageComponent implements OnInit {
  homeId!: string;
  permissions: string[] = [];
  isHomeOwner: boolean = false;

  sideBarItems = [
    {title : "Member", operations : this.permissions}
  ];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private homeService: HomeService,
    private authService: AuthService,
    private homeManagementService: HomeManagementService,
    private loadingService: LoadingService
  ) {}

  ngOnInit(): void {
    this.homeId = this.route.snapshot.paramMap.get('id') || '';
    this.homeManagementService.setHomeId(this.homeId);

    this.loadData().then(() => {
      this.updateSideBarItems();
      this.redirectBasedOnPermissions();
    });
  }

  async loadData(): Promise<void> {
    await this.getPermissions();
    await this.isTheHomeOwner();
  }

  async isTheHomeOwner(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.loadingService.show();
      this.homeService.userIsTheHomeOwner(this.homeId).subscribe({
        next: (response) => {
          this.loadingService.hide();
          this.isHomeOwner = response.data;
          resolve();
        },
        error: (err) => {
          console.error(err);
          reject(err);
        }
      });
    });
  }

  async getPermissions(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.loadingService.show();
      this.homeService.getMemberPermissions(this.homeId).subscribe({
        next: (response) => {
          this.loadingService.hide();
          const permissionsArray = response.data;
          this.permissions = permissionsArray.map((permission: any) => 
            permission.replace(/([A-Z])/g, ' $1').trim()
          );

          this.permissions = this.permissions.filter(permission => 
            !["Create Room", "Receive Notifications"].includes(permission)
          );

          resolve();
        },
        error: (err) => {
          console.error(err);
          reject(err);
        }
      });
    });
  }

  updateSideBarItems(): void {
    this.sideBarItems = [
      ...(this.isHomeOwner ? [{ title: "Home Owner", operations: ["Add Member", "List Members", "Create Room"] }] : []),
      ...(this.permissions.length > 0 ? [{ title: "Member", operations: this.permissions }] : [])
    ];
  }

  redirectBasedOnPermissions(): void {
    if (this.permissions.length === 0) {
      this.router.navigate(['welcome-no-permissions'], { relativeTo: this.route });
    }
  }
}