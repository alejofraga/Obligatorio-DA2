import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { UserRoleService } from "../../../../backend/services/userRole.service";
import { LoadingService } from "../../../../backend/services/loading.service";
import { UserService } from "../../../../backend/services/user.service";
import { AuthService } from "../../../../backend/services/auth.service";
import { SharedService } from "../../../../backend/services/shared.service";

@Component({
  selector: "app-main-page",
  templateUrl: "./main-page.component.html",
  styles: ``,
})
export class MainPageComponent implements OnInit {
  userCards: string[] = [];
  userEmail: string = "";
  defaultImageUrl = "";

  constructor(
    private router: Router,
    private roleService: UserRoleService,
    private loadingService : LoadingService,
    private userService: UserService,
    private authService: AuthService,
    private sharedService: SharedService
  ) {}

  ngOnInit(): void {
    this.loadingService.show();
    let emailResponse = this.authService.getEmail();
    if (emailResponse) {
      this.userEmail = emailResponse;
    }
    this.defaultImageUrl = this.userService.getDefaultProfilePicture();
    this.roleService.loadUserRoles().subscribe({
      next: () => {
        this.userCards = this.roleService.getUserRoles();
        if (!this.userCards.includes("HomeOwner")) {
          this.userCards.push("join-home-owner");
        }
        this.loadingService.hide();
      },
      error: (err) => {
        console.error("Error al obtener roles:", err);
      },
    });
  }

  goToAdmin = () => {
    this.router.navigate(["/admin"]);
  };

  goToCompanyOwner = () => {
    this.router.navigate(["/company-owner"]);
  };

  goToHomeOwner = () => {
    this.router.navigate(["/home-owner"]);
  };
  
  goToJoinHomeOwner = () => {
    const userConfirmed = confirm("This process is irreversible. Are you sure you want to continue?");
  
    if (userConfirmed) {
      const wantsToAddPhoto = confirm("Would you like to add a photo to your profile? If you dont, we will use a default image.");
        const imageData = {
          ProfilePicturePath: this.defaultImageUrl
        }
        this.confirmJoinHomeOwner();
        this.userService.updateProfilePicture(this.userEmail, imageData).subscribe(response => {
          if (wantsToAddPhoto) {
            console.log('entre');
            this.updateProfilePicture();
          }
          console.log(response);
        }, error => {
          console.error('Error al actualizar foto de perfil:', error);
        });
    }
  }

  confirmJoinHomeOwner = () => {
    this.roleService.addUserRole('HomeOwner').subscribe(response => {
      this.userCards.push('HomeOwner');
      this.userCards = this.userCards.filter(card => card !== 'join-home-owner');
      this.sharedService.emitRoleUpdated(); 
    }, error => {
      console.error('Error al agregar rol:', error);
    });
  }

  updateProfilePicture = () => {
    this.router.navigate(["/update-profile-picture"]);
  }

  homeOwnerImageUrl = "home-owner.jpg";
  adminImageUrl = "admin.jpg";
  companyOwnerImageUrl = "company-owner.jpg";
  joinHomeOwnerImageUrl = "join-home-owner.jpg";

  homeOwnerDescription =
    "Homeowners can create and manage their homes within the system, receive important notifications related to their household, and manage household members, ensuring a secure and connected environment.";
  adminDescription =
    "Admins can manage user accounts by listing all accounts, creating new accounts for company owners and other admins, and deleting admin accounts when necessary.";
  companyOwnerDescription =
    "Company owners are responsible for establishing their company within the system. They can register devices to facilitate service offerings and manage their company’s connected infrastructure.";
  joinHomeOwnerDescription =
    "Now you can also use your account to manage your home, by clicking on the button below you will configure your account to be a homeowner. Note that this operation is irreversible.";
}
