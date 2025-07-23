// StockTransaction model
export interface StockTransaction {
  stockTxnId: number; // Stok hareketi ID'si
  transactionType: 'Entry' | 'Exit'; // Hareket türü (örneğin, 'Giriş', 'Çıkış')
  quantity: number; // Miktar
  produxtName: string;
  RelatedOrderId?: number|null; // Açıklama
  createdAt: string; // Oluşturulma tarihi
}
