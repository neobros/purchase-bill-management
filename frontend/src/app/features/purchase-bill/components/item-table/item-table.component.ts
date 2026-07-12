import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PurchaseBillItem } from '../../../../core/models/purchase-bill.model';

@Component({
  selector: 'app-item-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './item-table.component.html',
  styleUrl: './item-table.component.scss',
})
export class ItemTableComponent {
  @Input() items: PurchaseBillItem[] = [];
  @Output() remove = new EventEmitter<number>();
  @Output() edit = new EventEmitter<PurchaseBillItem>();
}
