import React from "react";
import { Link, useNavigate } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import { logout } from "../redux/slices/authSlice";
import { Navbar, Nav, Container, Button } from "react-bootstrap";

function AppNavbar() {
  const { token, user } = useSelector((state) => state.auth);
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const handleLogout = () => {
    dispatch(logout());
    navigate("/login");
  };

  return (
    <Navbar bg="dark" variant="dark" expand="lg" className="fancy-navbar">
      <Container>
        <Navbar.Brand as={Link} to="/" className="fancy-logo">
          🏨heaven on earth
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse>
          {token && (
            <Nav className="me-auto">
              <Nav.Link as={Link} to="/camere" className="fancy-link">
                Camere
              </Nav.Link>
              <Nav.Link as={Link} to="/storico" className="fancy-link">
                Le mie prenotazioni
              </Nav.Link>
              <Nav.Link as={Link} to="/profilo" className="fancy-link">
                Profilo
              </Nav.Link>
              {user?.role === "Admin" && (
                <>
                  <Nav.Link as={Link} to="/admin/camere" className="fancy-link">
                    Gestione Camere
                  </Nav.Link>
                  <Nav.Link as={Link} to="/admin/prenotazioni" className="fancy-link">
                    Gestione Prenotazioni
                  </Nav.Link>
                  <Nav.Link as={Link} to="/admin/servizi" className="fancy-link">
                    Gestione Servizi
                  </Nav.Link>
                  <Nav.Link as={Link} to="/admin/utenti" className="fancy-link">
                    Gestione Utenti
                  </Nav.Link>
                </>
              )}
            </Nav>
          )}
          <Nav>
            {token ? (
              <Button variant="outline-light" onClick={handleLogout} className="fancy-button">
                Logout
              </Button>
            ) : (
              <>
                <Nav.Link as={Link} to="/login" className="fancy-link">
                  Login
                </Nav.Link>
                <Nav.Link as={Link} to="/register" className="fancy-link">
                  Register
                </Nav.Link>
              </>
            )}
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
}

export default AppNavbar;