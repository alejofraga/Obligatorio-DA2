import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { UserRoleService } from '../../backend/services/userRole.service';

@Injectable({
    providedIn: 'root'
})
export class HomeOwnerGuard implements CanActivate {
    constructor(private userRoleService: UserRoleService, private router: Router) {}

    canActivate(): boolean {
        console.log("roles: ", this.userRoleService.getUserRoles());
        if (this.userRoleService.getUserRoles().includes("HomeOwner")) {
            return true;
        } else {
            this.router.navigate(['']);
            return false;
        }
    }
    
}