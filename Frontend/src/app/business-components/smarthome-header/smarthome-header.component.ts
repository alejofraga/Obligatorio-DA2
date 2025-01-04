import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../backend/services/auth.service';
import { UserService } from '../../../backend/services/user.service';
import { SharedService } from '../../../backend/services/shared.service';
import { CommonModule } from '@angular/common';
import { LoadingService } from '../../../backend/services/loading.service';

@Component({
  selector: 'app-header',
  templateUrl: './smarthome-header.component.html',
  styleUrls: [],
  standalone: true,
  imports: [CommonModule]
})
export class SmartHomeHeaderComponent implements OnInit {
  userEmail: string | null = null;
  profilePicturePath: string = "";
  roles: string[] = [];
  dataLoaded: boolean = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private userService: UserService,
    private sharedService: SharedService,
    private loadingService: LoadingService
  ) {}

  ngOnInit(): void {
    this.loadUserData();
    this.sharedService.profilePictureUpdated$.subscribe(() => {
      this.loadUserData();
    });
    this.sharedService.roleUpdated$.subscribe(() => {
      this.loadUserData();
    });
  }

  async loadUserData(): Promise<void> {
    try {
      this.userEmail = await this.getEmail();
      if (this.userEmail) {
        this.profilePicturePath = await this.getProfilePath(this.userEmail);
      }
      this.roles = await this.getRoles();
      this.dataLoaded = true;
    } catch (error) {
      console.error('Error loading user data:', error);
    }
  }

  getEmail(): Promise<string | null> {
    return new Promise((resolve, reject) => {
      const email = this.authService.getEmail();
      if (email) {
        resolve(email);
      } else {
        reject('No email found');
      }
    });
  }

  getProfilePath(email: string): Promise<string> {
    return new Promise((resolve, reject) => {
      this.loadingService.show();
      this.userService.getProfilePicture(email).subscribe({
        next: (response) => {
          this.loadingService.hide();
          resolve(response.data);
        },
        error: (err) => {
          reject(err);
        }
      });
    });
  }

  getRoles(): Promise<string[]> {
    return new Promise((resolve, reject) => {
      const roles = this.authService.getRoles();
      if (roles) {
        resolve(roles);
      } else {
        reject('No roles found');
      }
    });
  }

  changeProfilePicture() {
    this.router.navigate(["/update-profile-picture"]);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  goToMainPage = () => {
    this.router.navigate(['/']);
  };
}