import { Component, OnInit } from "@angular/core";
import { UserService } from "../../../../backend/services/user.service";
import { CompanyStateService } from "../../../../backend/services/companyState.service";

@Component({
  selector: "app-company-owner-page",
  templateUrl: "./company-owner-page.component.html",
  styles: ``,
})
export class CompanyOwnerPageComponent implements OnInit {
  constructor(
    private userService: UserService,
    private companyStateService: CompanyStateService
  ) {}

  sideBarItems = [
    { title: "User", operations: ["List devices", "List supported devices"] },
    { title: "Company Owner", operations: [] },
  ];

  ngOnInit(): void {
    this.userService.loadHasCompany().subscribe((hasCompany: boolean) => {
      this.companyStateService.setHasCompany(hasCompany);
    });
  
    this.companyStateService.hasCompany$.subscribe((hasCompany) => {
      if (hasCompany) {
        this.sideBarItems[1].operations = [
          "Device registration",
          "Import devices",
        ];
      } else {
        this.sideBarItems[1].operations = ["Create Company"];
      }
    });
  }
}
