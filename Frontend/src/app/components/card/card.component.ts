import { Component, Input } from '@angular/core';
import { ButtonComponent } from '../button/button.component';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styles: ``,
  standalone: true,
  imports: [ButtonComponent]
})
export class CardComponent {

  @Input({ required: true }) description!: string;
  @Input({ required: true }) buttonText!: string;
  @Input({ required: true }) buttonAction!: (arg: any) => any;
  @Input({ required: true }) imageUrl!: string;
  @Input({ required: true }) title!: string;

  buttonClass = 'btn btn-primary';

}
