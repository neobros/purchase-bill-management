import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import {
  FinancialSummary,
  ItemSummary,
  TaxSummary,
} from '../../../../core/models/purchase-bill.model';

@Component({
  selector: 'app-summary-panel',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './summary-panel.component.html',
  styleUrl: './summary-panel.component.scss',
})
export class SummaryPanelComponent {
  @Input() itemSummary: ItemSummary = { totalItems: 0, totalQty: 0 };
  @Input() financialSummary: FinancialSummary = {
    grossTotal: 0,
    itemDiscount: 0,
    overallDiscount: 0,
    totalDiscounts: 0,
    totalBeforeTax: 0,
  };
  @Input() taxSummary: TaxSummary = { totalTax: 0, suspendedTax: 0 };
  @Input() netTotal = 0;
}
