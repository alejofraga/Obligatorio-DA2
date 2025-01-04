import { Component, OnInit } from "@angular/core";
import { CompanyService } from "../../../backend/services/company.service";
import { LoadingService } from "../../../backend/services/loading.service";

@Component({
  selector: "app-listing-companies",
  templateUrl: "./listing-companies.component.html",
  styles: ``,
})
export class ListingCompaniesComponent implements OnInit {
  dataObject: any = {};
  categories: any[] = [
    { name: "ownerFullName", alias: "Owner Fullname" },
    { name: "companyName", alias: "Company name" },
    { name: "rut", alias: "RUT" },
    { name: "ownerEmail", alias: "Owner email" },
  ];
  options: string[] = ["companyName", "ownerFullname"];
  warningMessage: string = "";
  offsetChange = 5;
  limit = 5;
  offset = 0;
  hasMoreCompanies: boolean = true;
  errorMessage: string = "";

  constructor(private companyService: CompanyService, private loadingService: LoadingService) {}

  ngOnInit() {
    this.fetchData();
  }

  fetchData(query?: string) {
    const fullQuery = query
      ? `${query}&offset=${this.offset}&limit=${this.limit}`
      : `offset=${this.offset}&limit=${this.limit}`;
      this.loadingService.show();
    this.companyService.getCompanies(fullQuery).subscribe({
      next: (response) => {
        this.dataObject = { ...response, originalData: response.data };
        this.hasMoreCompanies = response.data.length === this.limit;
        this.loadingService.hide();
      },
      error: (error: any) => {
        console.error("Error fetching companies:", error);
        this.errorMessage =
          error.details || "An error occurred while fetching companies";
        this.loadingService.hide();
      },
    });
  }
  onChangeOffset(newOffset: number) {
    if (newOffset < 0) {
      return;
    }
    this.offset = newOffset;
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

  private buildQueryString(filters: { [key: string]: string }): string {
    return Object.keys(filters)
      .map(
        (key) =>
          `${encodeURIComponent(key)}=${encodeURIComponent(filters[key])}`
      )
      .join("&");
  }
}
