export interface PurchaseBillItem {
  id: number;
  item: string;
  batch: string;
  standardCost: number;
  standardPrice: number;
  margin: number;
  qty: number;
  freeQty: number;
  discount: number;
  totalCost: number;
  totalSelling: number;
}

export interface ItemSummary {
  totalItems: number;
  totalQty: number;
}

export interface FinancialSummary {
  grossTotal: number;
  itemDiscount: number;
  overallDiscount: number;
  totalDiscounts: number;
  totalBeforeTax: number;
}

export interface TaxSummary {
  totalTax: number;
  suspendedTax: number;
}

/** Matches PurchaseBillItemDto — sent to POST/PUT /api/purchasebills. */
export interface PurchaseBillItemRequest {
  item: string;
  batch: string;
  standardCost: number;
  standardPrice: number;
  margin: number;
  qty: number;
  freeQty: number;
  discount: number;
}

export interface PurchaseBillCreateRequest {
  billDate: string;
  items: PurchaseBillItemRequest[];
}

/** Matches PurchaseBillItemResponseDto / PurchaseBillResponseDto. */
export interface PurchaseBillItemResponse extends PurchaseBillItemRequest {
  id: number;
  totalCost: number;
  totalSelling: number;
}

export interface PurchaseBillResponse {
  id: number;
  billDate: string;
  items: PurchaseBillItemResponse[];
  totalItems: number;
  totalQty: number;
  grossTotal: number;
  totalCost: number;
}
