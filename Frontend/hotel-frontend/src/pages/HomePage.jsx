import React from "react";
import { Carousel } from "react-bootstrap";
import { Link } from "react-router-dom";

function HomePage() {
  return (
    <div className="homepage">
      
      <Carousel className="fancy-carousel">
        <Carousel.Item>
          <img
            className="d-block w-100"
            src="/images/entrata.png"
            alt="Hotel di Lusso"
          />
          <Carousel.Caption>
            <h3 className="fancy-title">Benvenuti al nostro heaven on earth</h3>
            <p className="fancy-text">Un'esperienza unica e indimenticabile.</p>
          </Carousel.Caption>
        </Carousel.Item>
        <Carousel.Item>
          <img
            className="d-block w-100"
            src="/images/camerahomepage.png"
            alt="Camere di Lusso"
          />
          <Carousel.Caption>
            <h3 className="fancy-title">Camere di Lusso</h3>
            <p className="fancy-text">Comfort e stile per il tuo soggiorno.</p>
          </Carousel.Caption>
        </Carousel.Item>
        <Carousel.Item>
          <img
            className="d-block w-100"
            src="/images/spa.png"
            alt="Servizi Esclusivi"
          />
          <Carousel.Caption>
            <h3 className="fancy-title">Servizi Esclusivi</h3>
            <p className="fancy-text">Relax e benessere a portata di mano.</p>
          </Carousel.Caption>
        </Carousel.Item>
      </Carousel>

     
      <div className="container mt-5">
        <section className="text-center my-5 fancy-section" id="camere">
          <h2 className="fancy-title">Scopri le nostre Camere</h2>
          <p className="fancy-text">
            Camere eleganti e confortevoli per un soggiorno indimenticabile.
          </p>
          <Link to="/camere" className="btn btn-primary btn-lg fancy-button">
            Scopri di più
          </Link>
        </section>

        <section className="text-center my-5 fancy-section" id="servizi">
          <h2 className="fancy-title">Servizi Esclusivi</h2>
          <p className="fancy-text">
            Spa, ristoranti gourmet e molto altro per il tuo relax.
          </p>
          <Link to="/servizi" className="btn btn-primary btn-lg fancy-button">
            Scopri di più
          </Link>
        </section>
      </div>

     
      <footer className="fancy-footer">
        <div className="container text-center py-4">
          <p className="footer-text">
            © {new Date().getFullYear()} Haven on earth. Tutti i diritti riservati.
          </p>
          <p className="footer-text">
            Seguici su:
            <a href="#" className="footer-link"> Facebook</a> | 
            <a href="#" className="footer-link"> Instagram</a> | 
            <a href="#" className="footer-link"> Twitter</a>
          </p>
        </div>
      </footer>
    </div>
  );
}

export default HomePage;