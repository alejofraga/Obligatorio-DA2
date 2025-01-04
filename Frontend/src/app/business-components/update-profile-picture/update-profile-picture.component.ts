import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../../backend/services/auth.service';
import { UserService } from '../../../backend/services/user.service';
import { SharedService } from '../../../backend/services/shared.service';

@Component({
  selector: 'app-update-profile-picture',
  templateUrl: './update-profile-picture.component.html',
  styles: []
})
export class UpdateProfilePictureComponent implements OnInit {
  profilePicturePath: string = '';
  userEmail: string = '';
  dataLoaded: boolean = false;
  updateForm!: FormGroup;
  errorMessage: string = "";
  successMessage: string = "";

  formField: any = {
    path: {
      name: 'profilePicturePath',
    }
  };

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private fb: FormBuilder,
    private sharedService: SharedService
  ) {}

  ngOnInit(): void {
    this.loadUserData();
    this.updateForm = this.fb.group({
      profilePicturePath: ['']
    });
  }

  async loadUserData(): Promise<void> {
    try {
      let emailResponse = await this.getEmail();
      if (emailResponse) {
        this.userEmail = emailResponse;
      }
      if (this.userEmail) {
        this.profilePicturePath = await this.getProfilePath(this.userEmail);
      }
      this.dataLoaded = true;
    } catch (error) {
      console.error('Error loading user data:', error);
    }
  }

  getProfilePath(email: string): Promise<string> {
    return new Promise((resolve, reject) => {
      this.userService.getProfilePicture(email).subscribe({
        next: (response) => {
          resolve(response.data);
        },
        error: (err) => {
          reject(err);
        }
      });
    });
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

  onSubmit(): void {
    if (this.updateForm.valid) {
      const path = this.updateForm.get(this.formField.path.name)?.value;
      if (path) {
        const imageData = {
          ProfilePicturePath: path
        }
        this.userService.updateProfilePicture(this.userEmail, imageData).subscribe({
          next: (response) => {
            this.errorMessage = '';
            this.successMessage = response.message;
            this.sharedService.emitProfilePictureUpdated();
            this.profilePicturePath = path;
            this.updateForm.reset();
          },
          error: (err) => {
            this.errorMessage = err.details
            console.error('Error updating profile picture:', err);
          }
        });
      }
      else{
        this.successMessage = '';
        this.errorMessage = 'Please fill out the required fields.';
        return;
      }
    }
    else{
      this.successMessage = '';
      this.errorMessage = 'Please fill out the required fields.';
      return;
    }
  }
}