import {
  Component,
  Input,
  SimpleChanges,
  Output,
  EventEmitter,
} from "@angular/core";
import { CommonModule } from "@angular/common";
import environments from "../../../../environments";
import { Router } from "@angular/router";
import { ButtonComponent } from "../../button/button.component";

@Component({
  selector: "app-listing-table",
  templateUrl: "./listing-table.component.html",
  styles: [],
  standalone: true,
  imports: [CommonModule, ButtonComponent],
})
export class ListingTableComponent {
  @Input() categories: {name: string, alias: string}[] = [];
  @Input() dataObject: any = {};
  @Input() imageCategories: { category: string; alt: string }[] = [];
  @Input() emptyMessage: string = "No data available";
  @Input() actionCategories: {
    category: string;
    class: string;
    title: string;
    onClick: (item: any) => void;
    showWhen?: (item: any) => boolean;
  }[] = [];
  @Input() isClickeable: boolean = false;
  @Input() buttonIcon: string = '';
  @Output() rowClicked = new EventEmitter<any>();

  data: any[] = [];

  constructor(private router: Router) {}

  ngOnChanges(changes: SimpleChanges) {
    if (changes["dataObject"] && this.dataObject && this.dataObject.data) {
      this.data = this.dataObject.data;
    }
  }

  isImageCategory(category: any): boolean {
    return this.imageCategories.some((imgCat) => imgCat.category === category.name);
  }

  isActionCategory(category: any): boolean {
    return this.actionCategories.some((actCat) => actCat.category === category.name);
  }

  getAltText(category: any): string {
    const imgCat = this.imageCategories.find(
      (imgCat) => imgCat.category === category.name
    );
    return imgCat ? imgCat.alt : "";
  }

  getImageUrl(item: any, category: any): string {
    return `${item[category.name]}`;
  }

  onRowClick(item: any): void {
    this.rowClicked.emit(item);
  }

  getActionClass(category: any): string {
    const actCat = this.actionCategories.find(
      (actCat) => actCat.category === category.name
    );
    return actCat ? actCat.class : "";
  }

  getActionOnClick(item: any, category: any): any {
    const actCat = this.actionCategories.find(
      (actCat) => actCat.category === category.name
    );
    return actCat ? () => actCat.onClick(item) : null;
  }

  showActionWhen(item: any, category: any): boolean {
    const actCat = this.actionCategories.find(
      (actCat) => actCat.category === category.name
    );

    if (actCat == undefined || actCat == null) {
      return false;
    }

    if (actCat.showWhen == undefined || actCat.showWhen == null) {
      return true;
    }

    return actCat.showWhen(item);
  }

  getActionTitle(category: any): string {
    const actCat = this.actionCategories.find(
      (actCat) => actCat.category === category.name
    );
    return actCat ? actCat.title : "";
  }
}
