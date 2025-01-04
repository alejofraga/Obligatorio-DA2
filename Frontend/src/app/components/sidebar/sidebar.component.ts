import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

interface SidebarItem {
  title: string;
  operations: string[];
}

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styles: ``,
  standalone: true,
  imports: [CommonModule, RouterModule]
})
export class SidebarComponent {
  @Input({ required: true }) items!: SidebarItem[];

  getRouterLink(operation: string): string {
    return operation.replace(/\s+/g, '-').toLowerCase();
  }
}