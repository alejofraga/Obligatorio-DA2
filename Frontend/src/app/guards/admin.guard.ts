import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { UserRoleService } from "../../backend/services/userRole.service";

@Injectable({
  providedIn: "root",
})
export class AdminGuard implements CanActivate {
  constructor(
    private userRoleService: UserRoleService,
    private router: Router
  ) {}

  canActivate(): boolean {
    if (this.userRoleService.getUserRoles().includes("Admin")) {
      return true;
    } else {
      this.router.navigate([""]);
      return false;
    }
  }
}
