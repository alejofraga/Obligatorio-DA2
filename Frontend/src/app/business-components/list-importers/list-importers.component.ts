import { Component, OnInit } from "@angular/core";
import { ImportService } from "../../../backend/services/import.service";
import { Router } from "@angular/router";
import { LoadingService } from "../../../backend/services/loading.service";

@Component({
  selector: "app-list-importers",
  templateUrl: "./list-importers.component.html",
  styles: ``,
})
export class ListImportersComponent implements OnInit {
  dataObject: any = {};
  categories: any[] = [
    { name: "name", alias: "Name" }
  ];

  constructor(private importService: ImportService, private router: Router, private loadingService : LoadingService) {}

  ngOnInit() {
    this.fetchData();
  }

  fetchData() {
    this.loadingService.show();
    this.importService.getImporters().subscribe({
      next: (response) => {
        this.dataObject = { ...response, originalData: response.data };
        this.loadingService.hide();
      },
      error: (err) => {
        console.error('Error fetching importers:', err);
        this.loadingService.hide();
      }
    });
  }
  onRowClicked(item: any) {
    this.router.navigate(["/company-owner/importer/", item.name]);
  }
}
