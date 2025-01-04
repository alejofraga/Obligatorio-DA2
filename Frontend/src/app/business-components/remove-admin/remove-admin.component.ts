import { Component } from "@angular/core";
import { UserService } from "../../../backend/services/user.service";
import { Router } from "@angular/router";
import { LoadingService } from "../../../backend/services/loading.service";

@Component({
  selector: "app-remove-admin",
  templateUrl: "./remove-admin.component.html",
  styles: ``,
})
export class RemoveAdminComponent {
  dataObject: any = {};
  categories: any[] = [
    { name: "email", alias: "Email" },
    { name: "name", alias: "Name" },
    { name: "lastname", alias: "Lastname" },
    { name: "fullname", alias: "Fullname" },
    { name: "accountCreationDate", alias: "Creation date" },
    { name: "Action", alias: "" },
  ];
  actionCategories = [
    {
      category: "Action",
      class: "btn btn-danger",
      title: "Delete",
      onClick: (item: any) => this.deleteOption(item)
    },
  ];
  options: string[] = ["fullname"];
  warningMessage: string = "";
  offsetChange = 3;
  limit = 3;
  offset = 0;
  hasMoreUsers: boolean = true;
  successMessage: string = "";
  errorMessage: string = "";

  constructor(private userService: UserService, private router: Router, private loadingService: LoadingService) {}

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
    query = `${query}&role=Admin`;
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
      error: (error) => {
      this.errorMessage = error.details || "An error occurred while fetching users";
      this.loadingService.hide();
      }
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

  deleteOption(item: any) {
    const userConfirmed = confirm(
      "This process is irreversible. Are you sure you want to continue?"
    );

    if (userConfirmed) {
      this.removeAdmin(item);
    }
  }

  removeAdmin = (item: any) => {
    this.userService.removeAdmin({ Email: item.email }).subscribe({
      next: () => {
        this.successMessage = "Admin removed successfully";
        this.errorMessage = "";
        this.fetchData();
      },
      error: (error) => {
        this.errorMessage =
          error.details || "An error occurred while removing admin";
        this.successMessage = "";
      }
    });
  };
}
