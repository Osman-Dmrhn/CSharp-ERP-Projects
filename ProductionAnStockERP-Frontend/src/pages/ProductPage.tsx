// Dosya: src/pages/ProductPage.tsx

import React, { useEffect, useState, useCallback } from 'react';
import { getAllProducts, createProduct, updateProduct, deleteProduct } from '../api/ProductService';
import type { ProductDto } from '../models/ProductDtos/ProductDto';
import type { ProductCreateDto } from '../models/ProductDtos/ProductCreateDto';
import type { ProductUpdateDto } from '../models/ProductDtos/ProductUpdateDto';
import type { ProductFilters } from '../models/ProductDtos/ProductFilters';
import type { PaginationInfo } from '../models/LogDtos/PaginationInfo';
import { PaginationComponent } from '../components/Pagination';
import { Spinner, Alert, Table, Button, Modal, Form, Row, Col, Container } from 'react-bootstrap';

const emptyCreateProduct: ProductCreateDto = { name: '', description: '' };

// --- Düzenleme Modalı ---
interface EditProductModalProps {
  product: ProductDto;
  onSave: (data: ProductUpdateDto) => void;
  onHide: () => void;
}

const EditProductModal: React.FC<EditProductModalProps> = ({ product, onSave, onHide }) => {
  const [formData, setFormData] = useState<ProductUpdateDto>({
    name: product.name,
    description: product.description || '',
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = () => {
    onSave(formData);
  };

  return (
    <Modal show onHide={onHide} centered>
      <Modal.Header closeButton><Modal.Title>Ürün Güncelle</Modal.Title></Modal.Header>
      <Modal.Body>
        <Form>
          <Form.Group className="mb-3">
            <Form.Label>Ürün Adı</Form.Label>
            <Form.Control type="text" name="name" value={formData.name} onChange={handleChange} required/>
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Açıklama</Form.Label>
            <Form.Control as="textarea" rows={3} name="description" value={formData.description} onChange={handleChange} />
          </Form.Group>
        </Form>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onHide}>İptal</Button>
        <Button variant="primary" onClick={handleSubmit}>Kaydet</Button>
      </Modal.Footer>
    </Modal>
  );
};

// --- Ana Sayfa Bileşeni ---
const ProductPage: React.FC = () => {
  const [products, setProducts] = useState<ProductDto[]>([]);
  const [pagination, setPagination] = useState<PaginationInfo | null>(null);
  const [filters, setFilters] = useState<ProductFilters>({ pageNumber: 1, pageSize: 10 });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [newProduct, setNewProduct] = useState<ProductCreateDto>(emptyCreateProduct);
  const [editingProduct, setEditingProduct] = useState<ProductDto | null>(null);

  const fetchProducts = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const result = await getAllProducts(filters);
      setProducts(result.products || []);
      setPagination(result.pagination);
    } catch (err) {
      setError('Ürünler alınırken bir hata oluştu.');
      setProducts([]);
    } finally {
      setLoading(false);
    }
  }, [filters]);

  useEffect(() => {
    fetchProducts();
  }, [fetchProducts]);

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault();
    const result = await createProduct(newProduct);
    if (result.success) {
      fetchProducts();
      setNewProduct(emptyCreateProduct);
    } else {
      setError(result.message);
    }
  };
  
  const handleDelete = async (id: number) => {
    if (window.confirm("Bu ürünü silmek istediğinizden emin misiniz? Bu ürüne bağlı siparişler varsa silme işlemi başarısız olabilir.")) {
      const result = await deleteProduct(id);
      if (result.success) {
        fetchProducts();
      } else {
        setError(result.message);
      }
    }
  };

  const handleUpdate = async (updateData: ProductUpdateDto) => {
    if (editingProduct) {
      const result = await updateProduct(editingProduct.productId, updateData);
      if (result.success) {
        fetchProducts();
        setEditingProduct(null);
      } else {
        setError(result.message);
      }
    }
  };

  const handlePageChange = (page: number) => {
    setFilters(prev => ({ ...prev, pageNumber: page }));
  };

  return (
    <Container fluid className="p-4">
      <div className="bg-white rounded shadow p-4 mb-4">
        <h2 className="text-primary mb-4">Yeni Ürün Oluştur</h2>
        <Form onSubmit={handleCreate}>
          <Row className="g-3">
            <Col md={4}>
              <Form.Control type="text" placeholder="Ürün Adı" value={newProduct.name} onChange={(e) => setNewProduct({ ...newProduct, name: e.target.value })} required />
            </Col>
            <Col md={6}>
              <Form.Control type="text" placeholder="Açıklama (İsteğe Bağlı)" value={newProduct.description} onChange={(e) => setNewProduct({ ...newProduct, description: e.target.value })} />
            </Col>
            <Col md={2}>
              <Button type="submit" className="w-100">Ekle</Button>
            </Col>
          </Row>
        </Form>
      </div>

      <div className="bg-white rounded shadow p-4">
        <h3 className="mb-3">Ürün Listesi</h3>
        {loading ? <div className="text-center"><Spinner animation="border" /></div> : error ? <Alert variant="danger">{error}</Alert> : (
          <>
            <Table striped bordered hover responsive>
              <thead className="table-dark">
                <tr>
                  <th>#ID</th>
                  <th>Ürün Adı</th>
                  <th>Açıklama</th>
                  <th>Oluşturma Tarihi</th>
                  <th className="text-center">İşlemler</th>
                </tr>
              </thead>
              <tbody>
                {products.map((product) => (
                  <tr key={product.productId}>
                    <td>{product.productId}</td>
                    <td>{product.name}</td>
                    <td>{product.description}</td>
                    <td>{new Date(product.createdAt).toLocaleDateString('tr-TR')}</td>
                    <td className="text-center">
                      <Button variant="warning" size="sm" className="me-2" onClick={() => setEditingProduct(product)}>Düzenle</Button>
                      <Button variant="danger" size="sm" onClick={() => handleDelete(product.productId)}>Sil</Button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </Table>
            <div className="d-flex justify-content-end">
              {pagination && <PaginationComponent pagination={pagination} onPageChange={handlePageChange} />}
            </div>
          </>
        )}
      </div>

      {editingProduct && <EditProductModal product={editingProduct} onSave={handleUpdate} onHide={() => setEditingProduct(null)} />}
    </Container>
  );
};

export default ProductPage;