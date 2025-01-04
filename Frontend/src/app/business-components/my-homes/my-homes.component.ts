import { Component, OnInit } from "@angular/core";
import { MeHomeService } from "../../../backend/services/me.home.service";
import { Router } from "@angular/router";
import { LoadingService } from "../../../backend/services/loading.service";

@Component({
  selector: "app-my-homes",
  templateUrl: "./my-homes.component.html",
  styles: ``,
})
export class MyHomesComponent implements OnInit {
  dataObject: any = {};
  categories: any[] = [
    { name: "name", alias: "Name" },
    { name: "id", alias: "ID" }
  ];
  infoMessage: string =
    "Click on the row of the house you wish to enter to the administration panel.";
  errorMessage: string = "";
  offsetChange = 3;
  limit = 3;
  offset = 0;
  hasMoreHomes: boolean = true;

  constructor(private meHomeService: MeHomeService, private router: Router,
    private loadingService: LoadingService
  ) {}

  ngOnInit() {
    this.fetchData();
  }

  fetchData(query?: string) {
    this.loadingService.show();
    const fullQuery = query
      ? `${query}&offset=${this.offset}&limit=${this.limit}`
      : `offset=${this.offset}&limit=${this.limit}`;

    this.meHomeService.getHomes(fullQuery).subscribe({
      next: (response) => {
        this.loadingService.hide();
        this.dataObject = { ...response, originalData: response.data };
        this.hasMoreHomes = response.data.length === this.limit;
        this.errorMessage = "";
      },
      error: (err) => {
        this.loadingService.hide();
        this.errorMessage = err.details;
      }
    });
  }

  onChangeOffset(newOffset: number) {
    if (newOffset < 0) {
      return;
    }
    this.offset = newOffset;
    this.fetchData();
  }

  onRowClick(item: any) {
    this.router.navigate([`/home-management/${item.id}`]);
  }
}
