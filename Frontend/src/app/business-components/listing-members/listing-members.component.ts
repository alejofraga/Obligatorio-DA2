import { Component, OnInit } from "@angular/core";
import { HomeService } from "../../../backend/services/home.service";
import { HomeManagementService } from "../../../backend/services/home-management.service";
import { LoadingService } from "../../../backend/services/loading.service";

@Component({
  selector: "app-listing-members",
  templateUrl: "./listing-members.component.html",
  styles: ``,
})
export class ListingMembersComponent implements OnInit {
  homeId!: string;
  dataObject: any = {};
  categories: any[] = [
    { name: "fullName", alias: "Fullname" },
    { name: "email", alias: "Email" },
    { name: "profilePicture", alias: "Profile picture" },
    { name: "canRecieveNotifications", alias: "Can recieve notifications" },
  ];
  options = ["fullName"];
  warningMessage: string = "";
  offsetChange = 5;
  limit = 5;
  offset = 0;
  hasMoreUsers: boolean = true;
  errorMessage: string = "";

  constructor(
    private homeService: HomeService,
    private homeManagementService: HomeManagementService,
    private loadingService: LoadingService
  ) {}

  ngOnInit(): void {
    this.homeManagementService.homeId$.subscribe((id) => {
      if (id) {
        this.homeId = id;
      }
    });
    this.fetchData(this.homeId);
  }

  fetchData(id: string, query?: string) {
    const fullQuery = query
      ? `${query}&offset=${this.offset}&limit=${this.limit}`
      : `offset=${this.offset}&limit=${this.limit}`;
    this.loadingService.show();
    this.homeService.getMembers(id, fullQuery).subscribe({
      next: (response) => {
        const transformedData = response.data.map((member: any) => ({
          fullName: `${member.name} ${member.lastname}`,
          email: member.email,
          profilePicture: member.profilePicturePath,
          canRecieveNotifications: member.homePermissions.includes(
            "ReceiveNotifications"
          ),
        }));
        this.dataObject = {
          ...response,
          data: transformedData,
          originalData: transformedData,
        };
        this.loadingService.hide();
        this.errorMessage = "";
      },
      error: (err) => {
        this.errorMessage = "An error occurred while fetching data.";
        this.loadingService.hide();
      },
    });
  }
}
