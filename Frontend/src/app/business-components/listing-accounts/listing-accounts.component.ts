import { Component, OnInit } from "@angular/core";
import { UserService } from "../../../backend/services/user.service";
import { LoadingService } from "../../../backend/services/loading.service";

@Component({
  selector: "app-listing-accounts",
  templateUrl: "./listing-accounts.component.html",
  styles: ``,
})
export class ListingAccountsComponent implements OnInit {
  dataObject: any = {};
  categories: any[] = [
    { name: "name", alias: "Name" },
    { name: "lastname", alias: "Lastname" },
    { name: "fullname", alias: "Full name" },
    { name: "roles", alias: "Roles" },
    { name: "accountCreationDate", alias: "Creation date" },
  ];
  options: string[] = ["fullname", "role"];
  warningMessage: string = "";
  offsetChange = 5;
  limit = 5;
  offset = 0;
  hasMoreUsers: boolean = true;
  errorMessage: string = "";

  constructor(
    private userService: UserService,
    private loadingService: LoadingService
  ) {}

  ngOnInit() {
    this.fetchData();
  }

  onFilterChange(filterValues: { [key: string]: string }) {
    const validFilters = Object.keys(filterValues)
      .filter((key) => filterValues[key] && filterValues[key].trim() !== "")
      .reduce((obj, key) => {
        (obj as { [key: string]: string })[key] = filterValues[key];
        return obj;
      }, {} as { [key: string]: string });

    const queryString = this.buildQueryString(validFilters);
    setTimeout(() => {
      this.offset = 0;
      this.fetchData(queryString);
    }, 500);
  }

  fetchData(query?: string) {
    const fullQuery = query
      ? `${query}&offset=${this.offset}&limit=${this.limit}`
      : `offset=${this.offset}&limit=${this.limit}`;
    this.loadingService.show();
    this.userService.getUsers(fullQuery).subscribe({
      next: (response) => {
        this.dataObject = { ...response, originalData: response.data };
        this.hasMoreUsers = response.data.length === this.limit;
        this.loadingService.hide();
      },
      error: (error: any) => {
        console.error("Error fetching users:", error);
        this.errorMessage = error.details;
        this.loadingService.hide();
      },
    });
  }

  private buildQueryString(filters: { [key: string]: string }): string {
    return Object.keys(filters)
      .map(
        (key) =>
          `${encodeURIComponent(key)}=${encodeURIComponent(filters[key])}`
      )
      .join("&");
  }

  onChangeOffset(newOffset: number) {
    if (newOffset < 0) {
      return;
    }
    this.offset = newOffset;
    this.fetchData();
  }
}
